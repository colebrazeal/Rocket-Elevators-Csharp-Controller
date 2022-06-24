namespace Commercial_Controller
{
    //Button on a floor or basement to go back to lobby
    public class FloorRequestButton
    {
        public int ID;
        public string status;
        public int floor;
        public string direction;

        public FloorRequestButton(int _ID, string _status, int _floor, string _direction)
        {
            ID = _ID;
            status = _status;
            floor = _floor;
            direction = _direction;
        }
    }
}