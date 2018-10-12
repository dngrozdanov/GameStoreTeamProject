﻿using GameStore.Core.Abstract;

namespace GameStore.Core.ConsoleSections.Abstract
{
    public interface ILoggerSection : ISection
    {
        void ShowLog(IConsoleManager consoleManager);
    }
}