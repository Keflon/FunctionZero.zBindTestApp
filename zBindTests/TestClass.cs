//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace zBindTests
//{
//    public class TestClass : INotifyPropertyChanged
//    {
//        public TestClass(TestClass child, int startValue)
//        {
//            Child = child;
//            TestIntResult = startValue;
//        }
//        private int _testIntResult;




//        public int TestIntResult
//        {
//            get
//            {
//                return _testIntResult;
//            }
//            set
//            {
//                if (value != _testIntResult)
//                    _testIntResult = value;
//                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TestIntResult)));
//            }
//        }

//        private TestClass _child;

//        public TestClass Child
//        {
//            get
//            {
//                return _child;
//            }
//            set
//            {
//                if (value != _child)
//                {
//                    _child = value;
//                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Child)));
//            }
//        }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//    }
//}
