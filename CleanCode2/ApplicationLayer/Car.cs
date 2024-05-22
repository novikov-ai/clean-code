namespace CleanCode2.ApplicationLayer
{

    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public void StartEngine()
        {
            // ...
        }

        public void Accelerate()
        {
            // ...
        }

        public void Brake()
        {
            // ...
        }

        public void OpenLeftDoor()
        {
            // ...
        }

        public void OpenRightDoor()
        {
            // ...
        }

        public void TurnOnLights()
        {
            // ...
        }

         public void TurnOffLights()
        {
            // ...
        }
    }

    public class ElectricCar : Car
    {
        public int BatteryLife { get; set; }

        public override void StartEngine()
        {
            Console.WriteLine("Electric car starting...");
        }

        public override void Accelerate()
        {
            Console.WriteLine("Electric car accelerating...");
        }

        public override void Brake()
        {
            Console.WriteLine("Electric car braking...");
        }
    }

    public class GasolineCar : Car
    {
        public int FuelCapacity { get; set; }

        public override void StartEngine()
        {
            Console.WriteLine("Gasoline car starting...");
        }

        public override void Accelerate()
        {
            Console.WriteLine("Gasoline car accelerating...");
        }

        public override void Brake()
        {
            Console.WriteLine("Gasoline car braking...");
        }
    }
}