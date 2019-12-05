using System;

namespace PhotoManager.Contracts.Logic
{
    public class WrongFileTypeException : Exception
    {
        public WrongFileTypeException(string message):base(message){}
    }
}