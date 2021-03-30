//Script created by McCrodi, memory values found by McCrodi
//The values, while not perfect, seem to work decently well

state("Lava-Win64-Shipping")
{
    byte Loading : 0x03C34B70, 0x718; //First part of the loading screen, showing the name of the level and such
	byte Swirl : 0x0416E510, 0x7C0, 0xC0, 0x2F8; //Second part of the loading screen, showing the blue swirl
	//The memory value for Swirl also interacts with other elements in the game sometimes but it's not an issue here because both values have to be combined to count as a load
}

init
{
vars.startofload = 0.0; //Sets up start of the loading screen
vars.firstload = 0; //Indicates that there's a loading screen happening to not get another game time value for startofload
vars.setgametime = 0.0; //Value to set the game time
vars.setgametimeflag = false; //Indicates when to set new game time
vars.stoptimer = 0; //Indicates when to stop the timer
}

update
{
if(current.Loading == 0 && vars.firstload == 0)
	{
	vars.firstload = 1;
	vars.startofload = timer.CurrentTime.GameTime.Value.TotalMilliseconds; //Setting startofload as current game time
	vars.stoptimer = 1; //First change to this value
	}
if(current.Loading !=0 && vars.firstload == 1)
{
vars.firstload = 0; //When first part of loading screen is done, value is changed back to 0 for next loads
}
if(current.Swirl == 1 && vars.stoptimer == 1)
	{
	vars.setgametime = vars.startofload; //Setting game time to the saved value, rewinding the timer
	vars.setgametimeflag = true; //        ^
	vars.stoptimer = 2; //Stopping the timer
	}
if(vars.stoptimer == 2 && current.Swirl == 0)
	{
	vars.stoptimer = 0; //When the Swirl is done, unpause the timer
	}
}

gameTime
{
	if(vars.setgametimeflag)
	{
		vars.setgametimeflag = false;
		return TimeSpan.FromMilliseconds(vars.setgametime); //Changing the current time to the start of the load
	}
}

isLoading 
{
    if(vars.stoptimer == 2)
        return true; //Stopping the timer

    return false;
}