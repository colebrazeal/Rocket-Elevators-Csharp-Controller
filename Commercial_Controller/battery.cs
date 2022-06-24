using System;
using System.Collections.Generic;
using System.Linq;

namespace Commercial_Controller
{
    public class Battery
    {
            public int ID;
            public int amountOfFloors;
            public int amountOfBasements;
            public int amountOfElevatorPerColumn;
            public string status;
            public int floorRequestButtonID = 1;
            public int columnID = 1;
            public List<Column> columnsList;
            public List<FloorRequestButton> floorRequestsButtonsList;

        public Battery(int _ID, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            ID = _ID;
            status = "online";
            columnsList = new List<Column>();
            floorRequestsButtonsList = new List<FloorRequestButton>();

            if (_amountOfBasements > 0){
                this.createBasementFloorRequestButtons(_amountOfBasements);
                this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
                _amountOfColumns --;
        }

            createBasementFloorRequestButtons(_amountOfFloors);
            createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);
        }

        //---------------------------------Methods--------------------------------------------//

        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            List<int>servedFloors = new List<int>();
            int floor = -1;
            for(int i=0; i < _amountOfBasements; i++){
                servedFloors.Add(floor);
                floor --;
            }
            var column = new Column(columnID, "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloors, true);
            columnsList.Add(column);
            columnID ++;

        }

        public void createColumns(int _amountOfColumns, int _amountOfFloors, int _amountOfElevatorPerColumn)
        {
            int amountOfFloorsPerColumn = (int)Math.Ceiling((double)(_amountOfFloors / _amountOfColumns));
            int floor = 1;

            for(int i=0; i < _amountOfColumns; i++)
            {
                List<int>servedFloors = new List<int>();
                for(int j=0; j < amountOfFloorsPerColumn; j++)
                {
                    if(floor <= _amountOfFloors){
                        servedFloors.Add(floor);
                        floor ++;
                    }
                }

                Column column = new Column(columnID, "online", _amountOfFloors, _amountOfElevatorPerColumn, servedFloors, false);
                columnsList.Add(column);
                columnID++;
            }
        }

        public void createFloorRequestButtons(int _amountOfFloors)
        {
            int buttonFloor = 1;
            for(int i=0; i < _amountOfFloors; i++)
            {
                var floorRequestButton = new FloorRequestButton(floorRequestButtonID, "off", buttonFloor, "up");
                floorRequestsButtonsList.Add(floorRequestButton);
                buttonFloor ++;
                floorRequestButtonID ++;
            }
        }

        public void createBasementFloorRequestButtons(int _amountOfBasements)
        {
            int buttonFloor = -1;
            for(int i=0; i < _amountOfBasements; i++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, "off", buttonFloor, "down");
                floorRequestsButtonsList.Add(floorRequestButton);
                buttonFloor --;
                floorRequestButtonID ++;
            }
        }

        public Column findBestColumn(int requestedFloor)
        {
            Column chosenColumn = columnsList[0];
            foreach(Column column in columnsList)
            {
                if(column.servedFloorsList.Contains(requestedFloor))
                {
                    chosenColumn = column; 
                    break;
                } 
            }
            return chosenColumn;
        }
        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int requestedFloor, string _direction)
        {
            Column column = findBestColumn(requestedFloor); 
            Elevator elevator = column.findElevator(1, _direction);
            elevator.addNewRequest(1);
            elevator.move();

            elevator.addNewRequest(requestedFloor);
            elevator.move();
            
            return (column, elevator);
                    
        }
    }
}

