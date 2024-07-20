using LiveSplit.Crash4LoadRemover;
using LiveSplit.Crash4LoadRemover.Controls;
using LiveSplit.Crash4LoadRemover.Memory;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public class Crash4LoadRemoverComponent : IComponent
    {
        private TimerModel timer;
        private CrashMemory memory;
        private Crash4LoadRemoverSettings settings;
        private string GameName = "";
        private string GameCategory = "";
        private int NumberOfSplits = 0;
        private List<string> SplitNames;
        private LiveSplitState liveSplitState;
        private bool LastSplitSkip = false;
        private TimeSpan? loadStartTime;
        private int framesSinceStartOfLoad = 0;

        private bool timerStarted = false;

        FileStream log_file_stream = null;
        StreamWriter log_file_writer = null;
        private List<int> NumberOfLoadsPerSplit;

        public Crash4LoadRemoverComponent(LiveSplitState state)
        {
            settings = new Crash4LoadRemoverSettings(state);
            memory = new CrashMemory();
            memory.Loading.OnValueChange += OnLoadingChange;
            memory.Swirl.OnValueChange += OnSwirlChange;

            GameName = state.Run.GameName;
            GameCategory = state.Run.CategoryName;
            NumberOfSplits = state.Run.Count;
            SplitNames = new List<string>();

            foreach (var split in state.Run)
            {
                SplitNames.Add(split.Name);
            }

            liveSplitState = state;
            NumberOfLoadsPerSplit = new List<int>();
            InitNumberOfLoadsFromState();

            settings = new Crash4LoadRemoverSettings(state);
            timer = new TimerModel { CurrentState = state };
            timer.CurrentState.OnStart += timer_OnStart;
            timer.CurrentState.OnReset += timer_OnReset;
            timer.CurrentState.OnSkipSplit += timer_OnSkipSplit;
            timer.CurrentState.OnSplit += timer_OnSplit;
            timer.CurrentState.OnUndoSplit += timer_OnUndoSplit;
            timer.CurrentState.OnPause += timer_OnPause;
            timer.CurrentState.OnResume += timer_OnResume;

            Logging.Write("[Component] Component created.");
        }

        public string ComponentName => "Crash 4: IAT Load Remover (Memory-Based) (Steam Ver.)";
        public float HorizontalWidth => 0;
        public float VerticalHeight => 0;
        public float MinimumWidth => 0;
        public float MinimumHeight => 0;
        public float PaddingTop => 0;
        public float PaddingBottom => 0;
        public float PaddingLeft => 7;
        public float PaddingRight => 7;

        public IDictionary<string, Action> ContextMenuControls => null;

        private void timer_OnResume(object sender, EventArgs e)
        {
            timerStarted = true;
        }

        private void timer_OnPause(object sender, EventArgs e)
        {
            timerStarted = false;
        }

        void timer_OnStart(object sender, EventArgs e)
        {
            InitNumberOfLoadsFromState();
            timer.InitializeGameTime();
            timerStarted = true;

            //ReloadLogFile();
        }

        private void timer_OnUndoSplit(object sender, EventArgs e)
        {
            //If we undo a split that already has met the required number of loads, we probably want the number to reset.
            if (NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] >= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
            {
                NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] = 0;
            }
        }

        private void ReloadLogFile()
        {
            if (settings.SaveDetectionLog == false)
                return;


            Directory.CreateDirectory(settings.DetectionLogFolderName);

            string fileName = Path.Combine(settings.DetectionLogFolderName + "/", "Crash4LoadRemover_Log_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_") + settings.removeInvalidXMLCharacters(GameName) + "_" + settings.removeInvalidXMLCharacters(GameCategory) + ".txt");

            if (log_file_writer != null)
            {
                if (log_file_writer.BaseStream != null)
                {
                    log_file_writer.Flush();
                    log_file_writer.Close();
                    log_file_writer.Dispose();
                }
                log_file_writer = null;
            }


            log_file_stream = new FileStream(fileName, FileMode.Create);
            log_file_writer = new StreamWriter(log_file_stream);
            log_file_writer.AutoFlush = true;
            Console.SetOut(log_file_writer);
            Console.SetError(log_file_writer);

        }


        private void timer_OnSplit(object sender, EventArgs e)
        {
            //If we split, we add all remaining loads to the last split.
            //This means that the autosplitter now starts at 0 loads on the next split.
            //This is just necessary for manual splits, as automatic splits will always have a difference of 0.
            var loadsRequiredTotal = settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex - 1].Name);
            var loadsCurrentTotal = CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex - 1);
            NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] += loadsRequiredTotal - loadsCurrentTotal;

            LastSplitSkip = false;
        }


        private int CumulativeNumberOfLoadsForSplitIndex(int splitIndex)
        {
            int numberOfLoads = 0;

            for (int idx = 0; (idx < NumberOfLoadsPerSplit.Count && idx <= splitIndex); idx++)
            {
                numberOfLoads += NumberOfLoadsPerSplit[idx];
            }
            return numberOfLoads;
        }



        private void timer_OnReset(object sender, TimerPhase value)
        {
            timerStarted = false;
            LastSplitSkip = false;
            InitNumberOfLoadsFromState();

            if (log_file_writer != null)
            {
                if (log_file_writer.BaseStream != null)
                {
                    log_file_writer.Flush();
                    log_file_writer.Close();
                    log_file_writer.Dispose();
                }
                log_file_writer = null;
            }

        }

        private void timer_OnSkipSplit(object sender, EventArgs e)
        {
            LastSplitSkip = true;
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
        }

        private void InitNumberOfLoadsFromState()
        {
            NumberOfLoadsPerSplit = new List<int>();
            NumberOfLoadsPerSplit.Clear();

            if (liveSplitState == null)
            {
                return;
            }

            foreach (var split in liveSplitState.Run)
            {
                NumberOfLoadsPerSplit.Add(0);
            }
            NumberOfLoadsPerSplit.Add(99999);
        }


        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
        }

        public Control GetSettingsControl(LayoutMode mode)
        {
            return settings;
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            return settings.SaveSettings(document);
        }

        public void SetSettings(XmlNode settings)
        {
            this.settings.LoadSettings(settings);
        }

        private bool loading = false;
        private bool swirlLoading = false;
        private bool doneLoading = true;

        private void OnLoadingChange(byte oldLoading, byte newLoading)
        {
            if (timerStarted)
            {
                loadStartTime = timer.CurrentState.CurrentTime.GameTime;
                UpdateLoadingState(oldLoading, newLoading, true);
                UpdateGameTimerState();
                if (settings.AutoSplitterEnabled && !(settings.AutoSplitterDisableOnSkipUntilSplit && LastSplitSkip))
                {
                    if (loading && !swirlLoading)
                    {
                        NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex]++;

                        if (CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex) >= settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
                        {
                            timer.Split();
                        }
                    }
                }
            }
        }

        private void OnSwirlChange(byte oldSwirl, byte newSwirl)
        {
            framesSinceStartOfLoad = 0;
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}]Swirl - currentValue: {oldSwirl} - newValue: {newSwirl}");
            if (timerStarted)
            {
                UpdateSwirlState(oldSwirl, newSwirl, true);
                UpdateGameTimerStateSwirl();
            }
            loadStartTime = null;
        }

        private void UpdateGameTimerState()
        {
            /*When the second part of the loading starts, stop checking for this*/
            if (!swirlLoading)
            {
                timer.CurrentState.IsGameTimePaused = (loading);
            }
        }

        private void UpdateGameTimerStateSwirl()
        {
            timer.CurrentState.IsGameTimePaused = (swirlLoading);
        }

        private void UpdateLoadingState(byte oldState, byte newState, bool done)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}]Loading - oldState: {oldState} - newState: {newState}");
            loading = newState != 0;
            doneLoading = done;
        }

        private void UpdateSwirlState(byte oldState, byte newState, bool done)
        {

            swirlLoading = newState != 0;
            doneLoading = done;
            if (swirlLoading)
            {
                var currentGameTime = loadStartTime;
                timer.CurrentState.SetGameTime(currentGameTime);
            }
        }


        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (SplitsAreDifferent(state))
            {
                settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);

                //ReloadLogFile();
            }
            //if (loading)
            //    framesSinceStartOfLoad++;

            //if ((loading) && !doneLoading && framesSinceStartOfLoad >= settings.MaxFramesToWaitForSwirl) /*Arbitrary number of frames to wait*/
            //{
            //    var currentGameTime = timer.CurrentState.CurrentTime.GameTime;
            //    currentGameTime += timer.CurrentState.CurrentTime.RealTime - loadStartTime;
            //    framesSinceStartOfLoad = 0;
            //    timer.CurrentState.SetGameTime(currentGameTime);
            //    UpdateGameTimerState();
            //    loadStartTime = null;
            //}


            liveSplitState = state;

            UpdateMemoryRead();

            if (memory.ProcessHooked)
            {
                invalidator?.Invalidate(0, 0, width, height);
            }
        }

        private bool SplitsAreDifferent(LiveSplitState newState)
        {
            //If GameName / Category is different
            if (GameName != newState.Run.GameName || GameCategory != newState.Run.CategoryName)
            {
                GameName = newState.Run.GameName;
                GameCategory = newState.Run.CategoryName;
                return true;
            }

            //If number of splits is different
            if (newState.Run.Count != liveSplitState.Run.Count)
            {
                NumberOfSplits = newState.Run.Count;
                return true;
            }

            //Check if any split name is different.
            for (int splitIdx = 0; splitIdx < newState.Run.Count; splitIdx++)
            {
                if (newState.Run[splitIdx].Name != SplitNames[splitIdx])
                {
                    SplitNames = new List<string>();

                    foreach (var split in newState.Run)
                    {
                        SplitNames.Add(split.Name);
                    }
                    return true;
                }
            }
            return false;
        }



        public void UpdateMemoryRead()
        {
            bool processHooked = memory.HookProcess();

            if (!processHooked)
            {
                if (timerStarted)
                {
                    var currentGameTime = timer.CurrentState.CurrentTime.GameTime;
                    currentGameTime += timer.CurrentState.CurrentTime.RealTime - loadStartTime;
                    framesSinceStartOfLoad = 0;
                    UpdateLoadingState(1, 3, true);
                    timer.CurrentState.SetGameTime(currentGameTime);
                    UpdateGameTimerState();
                    loadStartTime = null;
                }
                return;
            }
            if (timerStarted)
                memory.Refresh();
        }

        public void Dispose()
        {
            Logging.Write("[Component] Component closing.");
        }
    }
}
