using System;
using System.Collections.Generic;
using System.Text;

namespace CloudDb
{
    interface IDynamoDbService
    {
        void CreateTable();
        void AddItem();
        void Updateitem();
        void DeleteItem();

    }
}
