using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Commercial_Controller
{
    public class Elevator
    {
        public int ID;
        public string status;
        public int amountOfFloors;
        public int bestScore;
        public string direction;
        public bool overweight;
        public int currentFloor;
        public string requestedDirection;
        public Door door;
        public List<int>floorRequestsList;
        public List<int> completedRequestsList;
        public Elevator(int _ID, string _status, int _amountOfFloors, int _currentFloor)
        {
            ID = _ID;
            status = _status;
            amountOfFloors = _amountOfFloors;
            currentFloor = _currentFloor;
            door = new Door(ID, "closed");
            floorRequestsList = new List<int>();
            completedRequestsList = new List<int>();
            direction = "stopped";
            overweight = false;

        }
        public void move()
        {
            while(floorRequestsList.Any())
            {
                int destination = floorRequestsList[0];
                status = "moving";
                if(currentFloor < destination)
                {
                    direction = "up";
                    sortFloorList();
                    while(currentFloor < destination)
                    {
                        currentFloor++;
                        int screenDisplay = currentFloor;
                    }
                }else if(currentFloor > destination)
                {
                    direction = "down";
                    sortFloorList();
                    while(currentFloor > destination)
                    {
                        currentFloor--;
                        int screenDisplay = currentFloor;
                    }
                }
                status = "stopped";
                operateDoors();
                
                completedRequestsList.Add(floorRequestsList[0]);               
                floorRequestsList.RemoveAt(0);
            }
            status = "idle";
        }

        public void sortFloorList()
        {
            if(direction == "up")
            {
                floorRequestsList.Sort((a,b) => a.CompareTo(b));
            }else{
                floorRequestsList.Sort((a,b) => b.CompareTo(a));
            }
        }
        
        public void operateDoors()
        {
            //door = new Door(ID, "closed");
            door.status = "opened";
        }

        public void addNewRequest(int requestedFloor)
        {
            if(!floorRequestsList.Contains(requestedFloor))
            {
                floorRequestsList.Add(requestedFloor);
            }

            if(currentFloor < requestedFloor)
            {
                direction = "up";
            }

            if(currentFloor > requestedFloor)
            {
                direction = "down";
            }
        }
    }
}