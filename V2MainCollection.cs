using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;

namespace labwork2
{
    public class V2MainCollection : IEnumerable
    {
        List<V2Data> ListData = new List<V2Data>();
        public int Count { get; set; } = 0;

        public V2Data this[int index]
        {
            get
            {
                return ListData[index];
            }

            set
            {
                ListData[index] = value;
                DataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace,
                    ListData[index].Freq_field));
            }
        }

        public void Add(V2Data item)
        {
            ListData.Add(item);
            ++Count;

            ListData[Count - 1].PropertyChanged += PropertyChangedEventHandler;

            DataChanged(this, new DataChangedEventArgs(ChangeInfo.Add,
                    item.Freq_field));
        }

        public bool Remove(string id, double w)
        {
            List<V2Data> items_deleted = ListData.FindAll(elem => elem.Description == id &&
                                                          elem.Freq_field == w);

            foreach (V2Data item in items_deleted)
            {
                item.PropertyChanged -= PropertyChangedEventHandler;
                DataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove,
                            item.Freq_field));
            }
            
            int removedCount = ListData.RemoveAll(elem => elem.Description == id &&
                                                  elem.Freq_field == w);
            Count -= removedCount;
            return removedCount > 0;
        }

        public void AddDefaults()
        {
            for (int i = 0; i < 2; ++i) {
                V2DataOnGrid v2_data_on_grid = new V2DataOnGrid(0.0, "Default info", new double[] { 0.01, 0.01 }, new int[] { 3, 3 });
                v2_data_on_grid.InitRandom(-10.0f, 10.0f);
                ListData.Add(v2_data_on_grid);
                V2DataCollection v2_data_collection = new V2DataCollection(0.0, "Default info");
                v2_data_collection.InitRandom(3, 10.0f, 10.0f, -10.0f, 10.0f);
                ListData.Add(v2_data_collection);
            }
        }

        public override string ToString()
        {
            string str = "";

            foreach (V2Data dataElem in ListData) {
                str += $"{dataElem}\n\n";
            }

            return str;
        }
        public double Averege => (from data in ListData
                                  from val in data
                                  select Complex.Abs(val.Value_field)).Average();

        public IEnumerable<DataItem> MaxDeviation => (from data in ListData
                                                      from val in data
                                                      orderby Math.Abs(Complex.Abs(val.Value_field) - Averege)
                                                      group val by val.Value_field into gr
                                                      select gr).Last();

        public IEnumerable<Vector2> DuplicateMeasurement => from data in ListData
                                                            from val in data
                                                            orderby val
                                                            group val.Coord_field by val.Coord_field into gr
                                                            where gr.Count() > 1
                                                            select gr.First();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ListData).GetEnumerator();
        }

        public delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

        public event DataChangedEventHandler DataChanged;

        void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs args)
        {
            DataChanged(sender, new DataChangedEventArgs(ChangeInfo.ItemChanged,
                        float.Parse(args.PropertyName)));
        }
    }
}