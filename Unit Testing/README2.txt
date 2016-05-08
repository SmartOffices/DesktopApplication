README2.txt: UNIT TESTING

To see the unit test that was performed for the robot's movement, please see UnitTestingScreenshots-RobotMovement.pdf.

In the BotMovement.cs file, the "a.ind" in under the Update() function must be changed into an integer less than 7 (the number of cubicles defined). 

To run the code, the BotMovement.cs file must be attached to the Bot object in Unity. Information on installing Unity can be found in SetupGuide.pdf in the Unity Setup section. The Bot object can found on the left side in the Hierarchy section of the Unity GUI. Simply drag and drop the script onto the name of the object to attach it.

On the top of the screen, click the play button. The robot will move towards the cubicle/object corresponding to the specified array index. To control which array index specifies which location, please see the Unity Section of the User Documentation.

----------------------
Updates After Demo 2
----------------------

TimesheetCreator.sln is the file that performs the unit testing for the automatic timesheet generation functionality of the security aspect of the the Smart Office System. When fed data, it populates an Excel sheet in the desired format.



