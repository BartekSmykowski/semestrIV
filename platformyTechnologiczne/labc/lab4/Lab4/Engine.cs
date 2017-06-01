using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Engine : IComparable<Engine>
    {
        public int Id { get; set; }
        public double Displacement { get; set; }
        public double HorsePower { get; set; }
        public String Model { get; set; }

        public Engine() { }
        public Engine(double displacement, double horsePower, string model) {
            this.Displacement = displacement;
            this.HorsePower = horsePower;
            this.Model = model;
        }

        public override string ToString() {
            return Model + " " + Displacement + " (" + HorsePower + " hp) ";
        }

        public int CompareTo(Engine obj)
        {
            if (!this.Model.Equals(obj.Model))
            {
                return this.Model.CompareTo(obj.Model);
            }
            else if (!this.Displacement.Equals(obj.Displacement))
            {
                return this.Displacement.CompareTo(obj.Displacement);
            }
            else if (!this.HorsePower.Equals(obj.HorsePower))
            {
                return this.HorsePower.CompareTo(obj.HorsePower);
            }
            return 0;
        }
    }
}
