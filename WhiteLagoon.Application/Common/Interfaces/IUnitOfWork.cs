﻿namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villas { get; }
        IVillaNumberRepository VillaNumbers { get; }
        IAmenityRepository Amenities { get; }
        void Save();
    }
}
