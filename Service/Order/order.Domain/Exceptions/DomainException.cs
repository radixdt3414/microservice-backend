﻿namespace order.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string msg): base($"Domain Exception: {msg}"){}
    }
}