using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Column
    {
        public int ID;
         public string status;
        public int amountOfFloors;
        public int callButtonID;
        public int _amountOfFloors;
        public int amountOfElevators;
        public List<int> servedFloorsList;
        public List<Elevator>elevatorsList;
        public List<CallButton>callButtonsList;
        public Column(int _ID, string _status, int _amountOfFloors, int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            ID = _ID;
            status = _status;
            amountOfFloors = _amountOfFloors;
            amountOfElevators =_amountOfElevators;
            elevatorsList = new List<Elevator>();
            callButtonsList = new List<CallButton>();
            servedFloorsList = _servedFloors;

            this.createElevators(_amountOfFloors, _amountOfElevators);
            this.createCallButtons(_amountOfFloors, _isBasement);
        }

        //---------------------------------Methods--------------------------------------------//

        public void createCallButtons(int _amountOfFloors, bool _isBasement)
        {
            if(_isBasement)
            {
                int buttonFloor = -1;
                for(int i=0; i < _amountOfFloors; i++)
                {
                    //CallButton callButton = new CallButton(callButtonID, "off", buttonFloor, "up");
                    CallButton callButton = new CallButton(callButtonID, "off", buttonFloor, "up");
                    callButton.callButtonID = callButtonID;
                    callButtonsList.Add(callButton);
                    buttonFloor --;
                    callButtonID ++;
                }
            } else {
                int buttonFloor = 1;
                for(int i=0; i < _amountOfFloors; i++)
                {
                    CallButton callButton = new CallButton(callButtonID, "off", buttonFloor, "down");
                    callButton.callButtonID = callButtonID;
                    callButtonsList.Add(callButton);
                    buttonFloor ++;
                    callButtonID ++;
                }
            }
        }

        public void createElevators(int _amountOfFloors, int _amountOfElevators)
        {
            int elevatorID = 1;

            for(int i=0; i < _amountOfElevators; i++)
            {
                 Elevator elevator = new Elevator(elevatorID, "idle", _amountOfFloors, 1);
                 elevatorsList.Add(elevator);
                 elevatorID++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int userPosition, string direction)
        {
           Elevator elevator = findElevator(userPosition, direction);
           elevator.addNewRequest(userPosition);
           elevator.move();

           elevator.addNewRequest(1);
           elevator.move();
           

           return elevator;
        }

        //We use a score system depending on the current elevators state. Since the bestScore and the referenceGap are 
        //higher values than what could be possibly calculated, the first elevator will always become the default bestElevator, 
        //before being compared with to other elevators. If two elevators get the same score, the nearest one is prioritized. Unlike
        //the classic algorithm, the logic isn't exactly the same depending on if the request is done in the lobby or on a floor.
        public Elevator findElevator(int requestedFloor, string _requestedDirection)
        {

            BestElevatorInformations bestElevatorInformations = new BestElevatorInformations()
            {
                bestScore = 6,
                referenceGap = 1000000,
                bestElevator = null
            };
            //Elevator bestElevator = new Elevator()
            // {
            //     bestScore = 6,
            //     referenceGap = 1000000,
            //     bestElevatorInformations = new Elevator()
            // };

            if(requestedFloor == 1)
            {
                foreach(Elevator elevator in elevatorsList)
                {
                    //The elevator is at the lobby and already has some requests. It is about to leave but has not yet departed
                    if(1 == elevator.currentFloor && elevator.status == "stopped"){
                         bestElevatorInformations = checkIfElevatorIsBetter(1, elevator, bestElevatorInformations, requestedFloor);
                        //The elevator is at the lobby and has no requests
                    } else if(1 == elevator.currentFloor && elevator.status == "idle"){
                         bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestElevatorInformations, requestedFloor);
                        //The elevator is lower than me and is coming up. It means that I'm requesting an elevator to go to a basement, and the elevator is on it's way to me.
                    } else if(1 > elevator.currentFloor && elevator.direction == "up"){
                         bestElevatorInformations = checkIfElevatorIsBetter(3, elevator, bestElevatorInformations, requestedFloor);
                        //The elevator is above me and is coming down. It means that I'm requesting an elevator to go to a floor, and the elevator is on it's way to me
                    } else if(1 < elevator.currentFloor && elevator.direction == "down"){
                         bestElevatorInformations = checkIfElevatorIsBetter(3, elevator, bestElevatorInformations, requestedFloor);
                        //The elevator is not at the first floor, but doesn't have any request
                    } else if(elevator.status == "idle"){
                         bestElevatorInformations = checkIfElevatorIsBetter(4, elevator, bestElevatorInformations, requestedFloor);
                        //The elevator is not available, but still could take the call if nothing better is found
                    }else{
                         bestElevatorInformations = checkIfElevatorIsBetter(5, elevator, bestElevatorInformations, requestedFloor);
                    }
                    // Elevator bestElevator = bestElevatorInformations.bestElevator;
                    // Elevator bestScore = bestElevatorInformations.bestScore;
                    // Elevator referenceGap = bestElevatorInformations.referenceGap;
                }
            } else {
                foreach(Elevator elevator in elevatorsList)
                {
                   //The elevator is at the same level as me, and is about to depart to the first floor
                   if(requestedFloor == elevator.currentFloor && elevator.status == "stopped" && _requestedDirection == elevator.direction){
                        bestElevatorInformations = checkIfElevatorIsBetter(1, elevator, bestElevatorInformations, requestedFloor);
                       //The elevator is lower than me and is going up. I'm on a basement, and the elevator can pick me up on it's way
                   }else if(requestedFloor > elevator.currentFloor && elevator.direction == "up" && _requestedDirection == elevator.direction){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestElevatorInformations, requestedFloor);
                       //The elevator is higher than me and is going down. I'm on a floor, and the elevator can pick me up on it's way
                   }else if(requestedFloor < elevator.currentFloor && elevator.direction == "down" && _requestedDirection == elevator.direction){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestElevatorInformations, requestedFloor);
                       //The elevator is idle and has no requests
                   }else if(elevator.status == "idle"){
                        bestElevatorInformations = checkIfElevatorIsBetter(4, elevator, bestElevatorInformations, requestedFloor);
                       //The elevator is not available, but still could take the call if nothing better is found
                   }else{
                        bestElevatorInformations = checkIfElevatorIsBetter(5, elevator, bestElevatorInformations, requestedFloor);
                   }
                    // Elevator bestElevator = bestElevatorInformations.bestElevator;
                    // Elevator bestScore = bestElevatorInformations.bestScore;
                    // Elevator referenceGap = bestElevatorInformations.referenceGap;
                }
            }
                return bestElevatorInformations.bestElevator;
        }
        public BestElevatorInformations checkIfElevatorIsBetter(int scoreToCheck, Elevator newElevator, BestElevatorInformations bestElevatorInformations, int floor)
        {
            if(scoreToCheck < bestElevatorInformations.bestScore)
            {
                bestElevatorInformations.bestScore = scoreToCheck;
                bestElevatorInformations.bestElevator = newElevator;
                bestElevatorInformations.referenceGap = Math.Abs(newElevator.currentFloor - floor);
            }else if(bestElevatorInformations.bestScore == scoreToCheck){
                int gap = Math.Abs(newElevator.currentFloor - floor);
                if(bestElevatorInformations.referenceGap > gap)
                {
                    bestElevatorInformations.bestScore = scoreToCheck;
                    bestElevatorInformations.bestElevator = newElevator;
                    bestElevatorInformations.referenceGap = gap;
                }
            }
            return bestElevatorInformations;
        }

    }
}