namespace Lab2;
public class Program
{
    private static int modelingTime = 300; // Время моделирования
    private static TankerQueue queue = new(5); // Очередь танкеров
    public static TugBoat tugBoat = new();
    private static int elapsedTime = 0; // Прошедшее время
    private static long avgFreeBerths = 0; // Среднее количество свободных причалов
    private static long avgTankerCount = 0; // Среднее количество танкеров в очереди
    private static int tanker = 0; //общее количество танкеров
    private static int avgTankerServiceTime = 0; //среднее время, проведенное танкером в порту

    private static List<Berth> berths = new();
    private static readonly bool[] tideSchedule = {
        false, false, false, false, true, true,
        true, false, false, false, false, false,
        true, true, true, true, true, false,
        false, false, false, false, false, false
    }; // прилив с 4 до 7, с 12 до 17

    public static void Main(string[] args)
    {
        for (int i = 0; i < 4; i++)
        {
            berths.Add(new Berth());
        }

        
        while (elapsedTime < modelingTime)
        {
            Console.WriteLine($"Время суток: {elapsedTime % 24}");
            queue.Call(elapsedTime); //проверка на появление нового танкера в очереди

            if (tugBoat.IsFree && tugBoat.IsTowingToBarge)
            {
                tugBoat.IsTowingToBarge = false;
                var barge = GetFirstFreeBarge();
                if (barge is not null)
                {
                    GetFirstFreeBarge()!.SetBusyTime(GetTotalBusyTime(tanker + 1)); //доставка танкера в место заливки на определенное время
                    tanker++;
                    queue.DecQueue(); //уменьшение очереди кораблей на 1
                }
            }

            if (queue.GetQueue() > 0 && GetAmountOfFreeBerths() > 0 && tideSchedule[elapsedTime % 24] && tugBoat.IsFree) //проверка на наличие свободных мест для заливки и танкеров в очереди
            {
                int towingTime = GetFillTime(1, 3);
                if (tideSchedule[(elapsedTime + towingTime) % 24])
                {
                    tugBoat.SetBusyTime(GetTugBoatTowingTime(true, towingTime), true);
                }
            }

            if(GetAmountOfTowingNeedBerths() > 0 && tugBoat.IsFree && tideSchedule[elapsedTime % 24])
            {
                int towingTime = GetFillTime(1, 3);
                if (tideSchedule[(elapsedTime + towingTime) % 24])
                {
                    tugBoat.SetBusyTime(GetTugBoatTowingTime(false, towingTime), false);
                    GetFirstTowingNeedBarge().IsFree = true;
                }
            }

            RecalculateBusyTime(); //уменьшение времени заливки на 1
            var a = (double)(berths.Count - GetAmountOfFreeBerths() + queue.GetQueue());
            avgTankerServiceTime += (berths.Count - GetAmountOfFreeBerths() + queue.GetQueue());
            avgFreeBerths += GetAmountOfFreeBerths();
            avgTankerCount += queue.GetQueue();
            elapsedTime++;
        }

        Console.WriteLine($"Среднее количество свободных причалов: {(double)avgFreeBerths / modelingTime:F5}");
        Console.WriteLine($"Среднее количество танкеров в очереди: {(double)avgTankerCount / modelingTime:F5}");
        Console.WriteLine($"Количество обслуженных танкеров: {tanker}");
        Console.WriteLine($"Среднее время обслуживания танкера: {(double)avgTankerServiceTime / modelingTime:F5}");
    }

    private static Berth? GetFirstFreeBarge()
    {
        return berths.FirstOrDefault(b => b.IsFree);
    }

    private static Berth GetFirstTowingNeedBarge()
    {
        return berths.First(b => b.NeedTowing);
    }

    private static void RecalculateBusyTime()
    {
        foreach (var berth in berths.Where(b => !b.IsFree))
        {
            berth.DecBusyTime();
        }
        tugBoat.DecBusyTime();
    }

    private static int GetAmountOfFreeBerths()
    {
        return berths.Count(b => b.IsFree);
    }

    private static int GetAmountOfTowingNeedBerths()
    {
        return berths.Count(b => b.NeedTowing);
    }

    private static int GetTugBoatTowingTime(bool isTowingToBarge, int towingTime)
    {

        if (isTowingToBarge) 
            Console.WriteLine($"Буксир ведет танкер на заливку, время доставки {towingTime}");
        else
            Console.WriteLine($"Буксир ведет танкер с заливки, время доставки {towingTime}");
        return towingTime;
    }

    private static int GetTotalBusyTime(int tanker)
    {
        int fillTime = GetFillTime(12, 48);
        Console.WriteLine($"Танкер {tanker} заливается, время заливки {fillTime}");
        return fillTime;
    }

    private static int GetFillTime(int min, int max)
    {
        Random random = new();
        return random.Next(min, max); 
    }
}