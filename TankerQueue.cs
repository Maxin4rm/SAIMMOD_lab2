using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2;

public class TankerQueue
{
    private Random random = new();
    private int queue = 0;
    private double dt;
    private int lastCall = 0;
    private int callInterval;

    public TankerQueue(double dt)
    {
        this.dt = dt;
        callInterval = RndE();
    }

    private int RndE()
    {
        double u;
        do
        {
            u = random.NextDouble();
        } while (u < 1E-32);
        return (int)Math.Round(-dt * Math.Log(u));
    }

    public void Call(int time)
    {
        if (time - lastCall >= callInterval)
        {
            queue++;
            lastCall = time;
            callInterval = RndE();
            Console.WriteLine("В очередь на заливку прибыло судно");
        }
    }

    public int GetQueue()
    {
        return queue;
    }

    public void DecQueue()
    {
        queue--;
    }
}
