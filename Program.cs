using System;


namespace labwork2
{
    class Program
    {
        static void Main(string[] args)
        {
            var main_collection = new V2MainCollection();
            main_collection.AddDefaults();
            Console.WriteLine(main_collection.ToString());

            main_collection.DataChanged += DataChangedEventHandler;

            V2DataOnGrid data_on_grid = new V2DataOnGrid(0, "New V2 data on grid",
                new double[] { 0.01, 0.01 }, new int[] { 5, 5 });

            main_collection.Add(data_on_grid);
            main_collection[3] = new V2DataCollection(1.0f, "Desc");
            main_collection[2].Description = "Service info";
            main_collection[2].Freq_field = 21.0f;
            main_collection.Remove("Service info", 21.0f);
        }

        static void DataChangedEventHandler(object sender, DataChangedEventArgs args)
        {
            Console.WriteLine(args.ToString());
        }
    }
}
