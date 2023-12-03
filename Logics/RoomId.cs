namespace CoinPusherServer.Logics;

using System;

public class RoomIdLogics
{
    public int DecideroomId()
    {
        Random randid=new System.Random();
        var roomid=randid.Next(100,999);

        return roomid;
    }
}