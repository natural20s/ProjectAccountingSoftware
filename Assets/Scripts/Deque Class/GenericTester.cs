using System;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using Sanford.Collections.Generic;

// "The variable "xxx" is assigned a value but never used
#pragma warning disable 0219

namespace DequeTest
{
    public class GenericTester
    {
        private const int ElementCount = 100;        

        public static void TestDeque(Deque<int> deque)
        {
            deque.Clear();
            System.Diagnostics.Debug.Assert(deque.Count == 0);

            PopulateDequePushFront(deque);
            PopulateDequePushBack(deque);
            TestPopFront(deque);
            TestPopBack(deque);
            TestContains(deque);
            TestCopyTo(deque);
            TestToArray(deque);
            TestClone(deque);
            TestEnumerator(deque);
        }

        private static void PopulateDequePushFront(Deque<int> deque)
        {
            deque.Clear();

            for(int i = 0; i < ElementCount; i++)
            {
                deque.PushFront(i);
            }

            System.Diagnostics.Debug.Assert(deque.Count == ElementCount);

            int j = ElementCount - 1;

            foreach(int i in deque)
            {
                System.Diagnostics.Debug.Assert(i == j);
                j--;
            }
        }

        private static void PopulateDequePushBack(Deque<int> deque)
        {
            deque.Clear();

            for(int i = 0; i < ElementCount; i++)
            {
                deque.PushBack(i);
            }

            System.Diagnostics.Debug.Assert(deque.Count == ElementCount);

            int j = 0;

            foreach(int i in deque)
            {
                System.Diagnostics.Debug.Assert(i == j);
                j++;
            }
        }

        private static void TestPopFront(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int j;

            for(int i = 0; i < ElementCount; i++)
            {
                j = (int)deque.PopFront();

                System.Diagnostics.Debug.Assert(j == i);
            }

            System.Diagnostics.Debug.Assert(deque.Count == 0);
        }

        private static void TestPopBack(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int j;

            for(int i = 0; i < ElementCount; i++)
            {
                j = (int)deque.PopBack();

                System.Diagnostics.Debug.Assert(j == ElementCount - 1 - i);
            }

            System.Diagnostics.Debug.Assert(deque.Count == 0);
        }

        private static void TestContains(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            for(int i = 0; i < deque.Count; i++)
            {
                System.Diagnostics.Debug.Assert(deque.Contains(i));
            }

            System.Diagnostics.Debug.Assert(!deque.Contains(ElementCount));
        }

        private static void TestCopyTo(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int[] array = new int[deque.Count];

            deque.CopyTo(array, 0);

            foreach(int i in deque)
            {
                System.Diagnostics.Debug.Assert(array[i] == i);
            }

            array = new int[deque.Count * 2];

            deque.CopyTo(array, deque.Count);

            foreach(int i in deque)
            {
                System.Diagnostics.Debug.Assert(array[i + deque.Count] == i);
            }

            array = new int[deque.Count];

            try
            {
                deque.CopyTo(null, deque.Count);
					
                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(array, -1);

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(array, deque.Count / 2);

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(array, deque.Count);

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(new int[10, 10], deque.Count);

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void TestToArray(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int[] array = deque.ToArray();
            int i = 0;

            foreach(int item in deque)
            {
                System.Diagnostics.Debug.Assert(item.Equals(array[i]));
                i++;
            }
        }

        private static void TestClone(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            Deque<int> deque2 = (Deque<int>)deque.Clone();

            System.Diagnostics.Debug.Assert(deque.Count == deque2.Count);

            IEnumerator<int> d2 = deque2.GetEnumerator();

            d2.MoveNext();

            foreach(int item in deque)
            {
                System.Diagnostics.Debug.Assert(item.Equals(d2.Current));

                d2.MoveNext();
            }
        }

        private static void TestEnumerator(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            IEnumerator<int> e = deque.GetEnumerator();

            try
            {
                //int item = e.Current;

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach(int item in deque)
                {
                    System.Diagnostics.Debug.Assert(e.MoveNext());
                }

                System.Diagnostics.Debug.Assert(!e.MoveNext());

                //int o = e.Current;

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                e.Reset();

                foreach(int item in deque)
                {
                    System.Diagnostics.Debug.Assert(e.MoveNext());
                }

                System.Diagnostics.Debug.Assert(!e.MoveNext());

                //int o = e.Current;

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.PushBack(deque.Count);

                e.Reset();

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                e.MoveNext();

                UnityEngine.Debug.LogError("Exception failed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
#pragma warning restore 0219
