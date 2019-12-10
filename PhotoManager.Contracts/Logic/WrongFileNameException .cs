using System;

namespace PhotoManager.Contracts.Logic
{
    public class WrongFileNameException : Exception
    {
        public WrongFileNameException(string message):base(message){}
    }
}