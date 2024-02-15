namespace DigitalMedia.Interfaces
{
    public interface ICombatCommunication
    {
        
        /// <summary>
        /// Used to tell the parryer (the person who parried) that they did indeed parry? Does this make sense. 
        /// </summary>
        public void DidParry();

        /// <summary>
        /// Tells the attacker that they were parried, not sure if we will need this one I keep changing my mind ngl. 
        /// </summary>
        public void WasParried();
    }
}