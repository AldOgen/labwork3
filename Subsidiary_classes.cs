using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;


namespace labwork2
{
    public struct DataItem : IComparable<DataItem>
    {
        public Complex Value_field { get; set; }
        public Vector2 Coord_field { get; set; }

        public DataItem(Complex value_field, Vector2 coord_field)
        {
            Value_field = value_field;
            Coord_field = coord_field;
        }
        public override string ToString() => $"Value of the electromagnetic field: {Value_field}\n" +
            $"Point coordinates: {Coord_field}\n";
        public string ToString(string format) => $"Value of the electromagnetic field: {Value_field.ToString(format)}\n" +
            $"Point coordinates: {Coord_field.ToString(format)}\n";
        public int CompareTo(DataItem other)
        {
            if (Coord_field.X == other.Coord_field.X)
            {
                return Math.Sign(Coord_field.Y - other.Coord_field.Y);
            }
            else
            {
                return Math.Sign(Coord_field.X - other.Coord_field.X);
            }

        }

    }

    public struct Grid1D
    {
        public double Step_grid { get; set; }
        public int Num_nodes_grid { get; set; }

        public Grid1D(double step_grid, int num_nodes_grid)
        {
            Step_grid = step_grid;
            Num_nodes_grid = num_nodes_grid;
        }
        public override string ToString() => $"Grid step: {Step_grid}\nNumber of grid nodes: {Num_nodes_grid}\n";
        public string ToString(string format) => $"Grid step: {String.Format(format, Step_grid)}\nNumber of grid nodes: {Num_nodes_grid}\n";
    }

    public abstract class V2Data : IEnumerable<DataItem>, INotifyPropertyChanged
    {
        private double freq_field = 0.0f;
        private string description = string.Empty;

        public double Freq_field
        {
            get
            {
                return freq_field;
            }
            set
            {
                freq_field = value;
                NotifyPropertyChanged(Freq_field.ToString());
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                NotifyPropertyChanged(Freq_field.ToString());
            }
        }

        public V2Data(double freq_field, string description)
        {
            Freq_field = freq_field;
            Description = description;
        }

        protected void NotifyPropertyChanged(string property_name_code_freq_field)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name_code_freq_field));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract Complex[] NearAverage(float eps);
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        public override string ToString() => $"Frequency field: {Freq_field}\n{Description}\n";
        public abstract IEnumerator<DataItem> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public enum ChangeInfo
    {
        ItemChanged,
        Add,
        Remove,
        Replace
    }

    public class DataChangedEventArgs
    {
        ChangeInfo Change_type { get; set; }
        double Double_var { get; set; }
        public DataChangedEventArgs(ChangeInfo change_type, double double_var)
        {
            Change_type = change_type;
            Double_var = double_var;
        }

        public override string ToString()
        {
            return Change_type.ToString() + " " + Double_var.ToString();
        }
    }
}
