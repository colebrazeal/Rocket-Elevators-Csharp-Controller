namespace Commercial_Controller
{
    //Button on a floor or basement to go back to lobby
    public class CallButton
    {
        public int callButtonID;
        public string status = "off";
        public int floor;
        public string direction;
        public CallButton(int _ID, string _status, int _floor, string _direction)
        {
            callButtonID = _ID;
            status = _status;
            floor = _floor;
            direction = _direction;

        }
    }
}