namespace Lopla.Language.Interfaces
{
    using Binary;

    public interface ILoplaIndexedValue : IValue
    {
        /// <summary>
        /// Change value of indexed element
        /// </summary>
        /// <param name="idx">number of element to be changed in the array</param>
        /// <param name="newValue">new value to be set up</param>
        void Set(int idx, Result newValue);

        Result Get(int idx);
    }
}