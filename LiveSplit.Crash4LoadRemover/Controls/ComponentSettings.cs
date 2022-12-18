using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Crash4LoadRemover.Controls
{
    public partial class Crash4LoadRemoverSettings : UserControl
    {
        public bool SaveDetectionLog = false;
        private Dictionary<string, XmlElement> AllGameAutoSplitSettings;
        private List<Control> dynamicAutoSplitterControls;
        private LiveSplitState liveSplitState = null;
        private AutoSplitData autoSplitData = null;
        public bool AutoSplitterEnabled = false;
        public bool AutoSplitterDisableOnSkipUntilSplit = false;
        public int MaxFramesToWaitForSwirl;

        public string DetectionLogFolderName = "Crash4LoadRemoverLog";

        public Crash4LoadRemoverSettings()
        {
            InitializeComponent();
        }

        public Crash4LoadRemoverSettings(LiveSplitState state)
        {
            InitializeComponent();
            //SaveDetectionLog = chkSaveDetectionLog.Checked;

            AllGameAutoSplitSettings = new Dictionary<string, XmlElement>();
            dynamicAutoSplitterControls = new List<Control>();
            CreateAutoSplitControls(state);
            liveSplitState = state;
            lblVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }

        private void CreateAutoSplitControls(LiveSplitState state)
        {
            autoSplitCategoryLbl.Text = "Category: " + state.Run.CategoryName;

            int splitOffsetY = 0;
            int splitSpacing = 50;

            int splitCounter = 0;
            autoSplitData = new AutoSplitData(removeInvalidXMLCharacters(state.Run.GameName), removeInvalidXMLCharacters(state.Run.CategoryName));

            foreach (var split in state.Run)
            {
                //Setup controls for changing AutoSplit settings
                var autoSplitPanel = new Panel();
                var autoSplitLbl = new Label();
                var autoSplitUpDown = new NumericUpDown();

                autoSplitUpDown.Value = 2;
                autoSplitPanel.BorderStyle = BorderStyle.Fixed3D;
                autoSplitPanel.Controls.Add(autoSplitUpDown);
                autoSplitPanel.Controls.Add(autoSplitLbl);
                autoSplitPanel.Location = new Point(0, splitOffsetY + splitSpacing * splitCounter);
                autoSplitPanel.Size = new Size(400, 39);

                autoSplitLbl.AutoSize = true;
                autoSplitLbl.Location = new Point(3, 10);
                autoSplitLbl.Size = new Size(199, 13);
                autoSplitLbl.TabIndex = 0;
                autoSplitLbl.Text = split.Name;

                autoSplitUpDown.Location = new Point(355, 8);
                autoSplitUpDown.Size = new Size(35, 20);
                autoSplitUpDown.TabIndex = 1;

                //Remove all whitespace to name the control, we can then access it in SetSettings.
                autoSplitUpDown.Name = removeInvalidXMLCharacters(split.Name);

                autoSplitUpDown.ValueChanged += (s, e) => AutoSplitUpDown_ValueChanged(autoSplitUpDown, e, removeInvalidXMLCharacters(split.Name));

                panelSplits.Controls.Add(autoSplitPanel);

                autoSplitData.SplitData.Add(new AutoSplitEntry(removeInvalidXMLCharacters(split.Name), 2));
                dynamicAutoSplitterControls.Add(autoSplitPanel);
                splitCounter++;
            }
        }


        private void AutoSplitUpDown_ValueChanged(object sender, EventArgs e, string splitName)
        {
            foreach (AutoSplitEntry entry in autoSplitData.SplitData)
            {
                if (entry.SplitName == splitName)
                {
                    entry.NumberOfLoads = (int)((NumericUpDown)sender).Value;
                    return;
                }
            }
        }


        public void ChangeAutoSplitSettingsToGameName(string gameName, string category)
        {
            gameName = removeInvalidXMLCharacters(gameName);
            category = removeInvalidXMLCharacters(category);

            foreach (var control in dynamicAutoSplitterControls)
            {
                tabPage2.Controls.Remove(control);
            }

            dynamicAutoSplitterControls.Clear();

            //Add current game to gameSettings
            XmlDocument document = new XmlDocument();

            var gameNode = document.CreateElement(autoSplitData.GameName + autoSplitData.Category);

            //var categoryNode = document.CreateElement(autoSplitData.Category);

            foreach (AutoSplitEntry splitEntry in autoSplitData.SplitData)
            {
                gameNode.AppendChild(CreateNode(document, splitEntry.SplitName, splitEntry.NumberOfLoads));
            }

            AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = gameNode;

            //otherGameSettings[]

            CreateAutoSplitControls(liveSplitState);

            //Change controls if we find the chosen game
            foreach (var gameSettings in AllGameAutoSplitSettings)
            {
                if (gameSettings.Key == gameName + category)
                {
                    var game_element = gameSettings.Value;

                    //var splits_element = game_element[autoSplitData.Category];
                    Dictionary<string, int> usedSplitNames = new Dictionary<string, int>();
                    foreach (XmlElement number_of_loads in game_element)
                    {
                        var up_down_controls = tabPage2.Controls.Find(number_of_loads.LocalName, true);

                        if (usedSplitNames.ContainsKey(number_of_loads.LocalName) == false)
                        {
                            usedSplitNames[number_of_loads.LocalName] = 0;
                        }
                        else
                        {
                            usedSplitNames[number_of_loads.LocalName]++;
                        }
                        NumericUpDown up_down = (NumericUpDown)up_down_controls[usedSplitNames[number_of_loads.LocalName]];

                        if (up_down != null)
                        {
                            up_down.Value = Convert.ToInt32(number_of_loads.InnerText);
                        }
                    }

                }
            }
        }

        public string removeInvalidXMLCharacters(string in_string)
        {
            if (in_string == null) return null;

            StringBuilder sbOutput = new StringBuilder();
            char ch;

            bool was_other_char = false;

            for (int i = 0; i < in_string.Length; i++)
            {
                ch = in_string[i];

                if ((ch >= 0x0 && ch <= 0x2F) ||
                    (ch >= 0x3A && ch <= 0x40) ||
                    (ch >= 0x5B && ch <= 0x60) ||
                    (ch >= 0x7B)
                    )
                {
                    continue;
                }

                //Can't start with a number.
                if (was_other_char == false && ch >= '0' && ch <= '9')
                {
                    continue;
                }
                sbOutput.Append(ch);
                was_other_char = true;
            }

            if (sbOutput.Length == 0)
            {
                sbOutput.Append("NULL");
            }

            return sbOutput.ToString();
        }

        public XmlNode SaveSettings(XmlDocument document)
        {
            XmlNode root = document.CreateElement("Settings");
            
            //root.AppendChild(CreateNode(document, "SaveDetectionLog", SaveDetectionLog));
            root.AppendChild(CreateNode(document, "AutoSplitEnabled", enableAutoSplitterChk.Checked));
            root.AppendChild(CreateNode(document, "AutoSplitDisableOnSkipUntilSplit", chkAutoSplitterDisableOnSkip.Checked));
            root.AppendChild(CreateNode(document, "MaxFramesToWaitForSwirl", framesToWaitForSwirl.Value));


            var splitsNode = document.CreateElement("AutoSplitGames");

            //Re-Add all other games/categories to the xml file
            foreach (var gameSettings in AllGameAutoSplitSettings)
            {
                if (gameSettings.Key != autoSplitData.GameName + autoSplitData.Category)
                {
                    XmlNode node = document.ImportNode(gameSettings.Value, true);
                    splitsNode.AppendChild(node);
                }
            }

            var gameNode = document.CreateElement(autoSplitData.GameName + autoSplitData.Category);
            foreach (AutoSplitEntry splitEntry in autoSplitData.SplitData)
            {
                gameNode.AppendChild(CreateNode(document, splitEntry.SplitName, splitEntry.NumberOfLoads));
            }
            AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = gameNode;
            splitsNode.AppendChild(gameNode);
            root.AppendChild(splitsNode);


            return root;
        }

        private XmlNode CreateNode(XmlDocument document, string tag, object value)
        {
            XmlNode node = document.CreateElement(tag);
            node.InnerText = value.ToString();

            return node;
        }

        public int GetCumulativeNumberOfLoadsForSplit(string splitName)
        {
            int numberOfLoads = 0;
            splitName = removeInvalidXMLCharacters(splitName);
            foreach (AutoSplitEntry entry in autoSplitData.SplitData)
            {
                numberOfLoads += entry.NumberOfLoads;
                if (entry.SplitName == splitName)
                {
                    return numberOfLoads;
                }
            }
            return numberOfLoads;
        }


        public int GetAutoSplitNumberOfLoadsForSplit(string splitName)
        {
            splitName = removeInvalidXMLCharacters(splitName);
            foreach (AutoSplitEntry entry in autoSplitData.SplitData)
            {
                if (entry.SplitName == splitName)
                {
                    return entry.NumberOfLoads;
                }
            }

            //This should never happen, but might if the splits are changed without reloading the component...
            return 2;
        }


        public void LoadSettings(XmlNode node)
        {
            lblVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            var element = (XmlElement)node;
            if (!element.IsEmpty)
            {
                if (element["AutoSplitEnabled"] != null)
                {
                    enableAutoSplitterChk.Checked = Convert.ToBoolean(element["AutoSplitEnabled"].InnerText);
                    enableAutoSplitterChk_CheckedChanged(this, null);
                }

                if (element["AutoSplitDisableOnSkipUntilSplit"] != null)
                {
                    chkAutoSplitterDisableOnSkip.Checked = Convert.ToBoolean(element["AutoSplitDisableOnSkipUntilSplit"].InnerText);
                }

                if (element["MaxFramesToWaitForSwirl"] != null)
                {
                    framesToWaitForSwirl.Value = Convert.ToDecimal(element["MaxFramesToWaitForSwirl"].InnerText);
                    MaxFramesToWaitForSwirl = Convert.ToInt32(framesToWaitForSwirl.Value);
                }
                else
                {
                    framesToWaitForSwirl.Value = MaxFramesToWaitForSwirl = 100; //Default value - For situations where you just update the dll and dont open the settings window.
                }

                //if (element["SaveDetectionLog"] != null)
                //{
                //    chkSaveDetectionLog.Checked = Convert.ToBoolean(element["SaveDetectionLog"].InnerText);
                //    SaveDetectionLog = chkSaveDetectionLog.Checked;
                //}

                if (element["AutoSplitGames"] != null)
                {
                    var auto_split_element = element["AutoSplitGames"];

                    foreach (XmlElement game in auto_split_element)
                    {
                        if (game.LocalName != autoSplitData.GameName)
                        {
                            AllGameAutoSplitSettings[game.LocalName] = game;
                        }
                    }

                    if (auto_split_element[autoSplitData.GameName + autoSplitData.Category] != null)
                    {
                        var game_element = auto_split_element[autoSplitData.GameName + autoSplitData.Category];
                        AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = game_element;
                        //var splits_element = game_element[autoSplitData.Category];
                        Dictionary<string, int> usedSplitNames = new Dictionary<string, int>();
                        foreach (XmlElement number_of_loads in game_element)
                        {
                            var up_down_controls = tabPage2.Controls.Find(number_of_loads.LocalName, true);

                            //This can happen if the layout was not saved and contains old splits.
                            if (up_down_controls == null || up_down_controls.Length == 0)
                            {
                                continue;
                            }

                            if (usedSplitNames.ContainsKey(number_of_loads.LocalName) == false)
                            {
                                usedSplitNames[number_of_loads.LocalName] = 0;
                            }
                            else
                            {
                                usedSplitNames[number_of_loads.LocalName]++;
                            }
                            NumericUpDown up_down = (NumericUpDown)up_down_controls[usedSplitNames[number_of_loads.LocalName]];

                            if (up_down != null)
                            {
                                up_down.Value = Convert.ToInt32(number_of_loads.InnerText);
                            }
                        }
                    }
                }
            }
        }

        private void ParseNode(XmlNode node, string tag, CheckBox checkbox)
        {
            XmlNode child = node.SelectSingleNode(".//" + tag);

            if (child != null)
            {
                checkbox.Checked = bool.Parse(child.InnerText);
            }
        }

        private void enableAutoSplitterChk_CheckedChanged(object sender, EventArgs e)
        {
            panelSplits.Enabled = chkAutoSplitterDisableOnSkip.Enabled = enableAutoSplitterChk.Checked;
            if (!chkAutoSplitterDisableOnSkip.Enabled)
                chkAutoSplitterDisableOnSkip.Checked = false;
            AutoSplitterEnabled = enableAutoSplitterChk.Checked;
        }

        private void chkAutoSplitterDisableOnSkip_CheckedChanged(object sender, EventArgs e)
        {
            AutoSplitterDisableOnSkipUntilSplit = chkAutoSplitterDisableOnSkip.Checked;
        }

        private void framesToWaitForSwirl_ValueChanged(object sender, EventArgs e)
        {
            MaxFramesToWaitForSwirl = Convert.ToInt32(framesToWaitForSwirl.Value);
        }
    }

    public class AutoSplitEntry
    {
        #region Public Fields

        public int NumberOfLoads = 2;
        public string SplitName = "";

        #endregion Public Fields

        #region Public Constructors

        public AutoSplitEntry(string splitName, int numberOfLoads)
        {
            SplitName = splitName;
            NumberOfLoads = numberOfLoads;
        }

        #endregion Public Constructors
    }

    public class AutoSplitData
    {
        #region Public Fields

        public string Category;
        public string GameName;
        public List<AutoSplitEntry> SplitData;

        #endregion Public Fields

        #region Public Constructors

        public AutoSplitData(string gameName, string category)
        {
            SplitData = new List<AutoSplitEntry>();
            GameName = gameName;
            Category = category;
        }

        #endregion Public Constructors
    }

}
