namespace ScriptEngine.Utils
{
    public class AtomicPositiveInteger
    {
        public int Number;

        public AtomicPositiveInteger(int n = 0)
        {
            Number = n;
        }

        public int IncrementAndGet()
        {
            return ++Number;
        }

        public int GetAndIncrement()
        {
            return Number++;
        }

        public void Increment()
        {
            Number++;
        }

        public int DecrementAndGet()
        {
            return Number - 1 >= 0 ? --Number : 0;
        }

        public int GetAndDecrement()
        {
            return Number - 1 >= 0 ? Number-- : 0;
        }

        public void Decrement()
        {
            Number--;
        }
    }
}