# Rocket-Elevators-Csharp-Controller

This program controls multiple Columns of elevators.

It sends an specific elevator when a user presses a button on a floor and it takes
a user to its desired floor when a button is pressed from the inside the elevator. The specified elevator is determined on the users location and required destination and the column need for users required destination

# Dependencies

To be able to try the program, you need to install dotnet test

- Dependency #1: "dotnet" is needed to test the elevator controller and make sure the code is operating properly to function the elevators with the controller


### Installation

As long as you have **.NET 6.0** installed on your computer, nothing more needs to be installed:

The code to run the scenarios is included in the Commercial_Controller folder, and can be executed there with:

`dotnet run <SCENARIO-NUMBER>`

### Running the tests

To launch the tests, make sure to be at the root of the repository and run:

`dotnet test`

With a fully completed project, you should get an output like:

![Screenshot from 2021-06-15 17-31-02](https://user-images.githubusercontent.com/28630658/122128889-3edfa500-ce03-11eb-97d0-df0cc6a79fed.png)

# Usage

Used to operate elevators in a residential building for people transporting through out the building

## Example
1. open terminal 
2. cd to the location of the file in your terminal and make sure you are in the right location by typing "ls" 
3. type "dotnet test" to test the elevator controller
4. should get a result saying "all tests passed"

