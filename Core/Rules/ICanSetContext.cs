// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using WillFrameworkPro.Core.Context;

namespace WillFrameworkPro.Core.Rules
{
    public interface ICanSetContext
    {
        void SetContext(IContext context);
    }
}