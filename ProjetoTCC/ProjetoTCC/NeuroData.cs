using System;

namespace ProjetoTCC
{
    public class NeuroData
    {
        public int Alpha1 { private set; get; }
        public int Alpha2 { private set; get; }
        public int Beta1 { private set; get; }
        public int Beta2 { private set; get; }
        public int Delta { private set; get; }
        public int Gamma1 { private set; get; }
        public int Gamma2 { private set; get; }
        public int Theta { private set; get; }
        public int Attention { private set; get; }
        public int Meditation { private set; get; }
        public int PoorSignal { private set; get; }

        public DateTime time;
        
        public NeuroData(int poorSignal,
            int attention, int meditation,
            int alpha1, int alpha2, 
            int beta1, int beta2, 
            int delta, int gamma1, 
            int gamma2, int theta)
        {
            this.Alpha1 = alpha1;
            this.Alpha2 = alpha2;
            this.Beta1 = beta1;
            this.Beta2 = beta2;
            this.Delta = delta;
            this.Gamma1 = gamma1;
            this.Gamma2 = gamma2;
            this.Theta = theta;
            this.Attention = attention;
            this.Meditation = meditation;
            this.PoorSignal = poorSignal;
            this.time = DateTime.UtcNow;
        }

        public double getTotalPower()
        {
            return this.Alpha1 + this.Alpha2
                + this.Beta1 + this.Beta2
                + this.Gamma1 + this.Gamma2
                + this.Delta + this.Theta;
        }
    }
}
