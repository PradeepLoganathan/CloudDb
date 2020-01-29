using System;
using System.Collections.Generic;
using System.Text;

namespace CloudDb
{
    class DynamoDbService : IDynamoDbService
    {
        public void AddItem()
        {
            Console.WriteLine("adding an item");
        }

        public void CreateTable()
        {
            throw new NotImplementedException();
        }

        public void DeleteItem()
        {
            throw new NotImplementedException();
        }

        public void Updateitem()
        {
            throw new NotImplementedException();
        }
    }
}
