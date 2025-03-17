using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    class workList<T>
    {

        private Node<T> head;

        public void Add(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node<T> current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        public bool Remove(T data)
        {
            if (head == null)
                return false;

            if (head.Data.Equals(data))
            {
                head = head.Next;
                return true;
            }

            Node<T> current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Equals(data))
                {
                    current.Next = current.Next.Next;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public string Print()
        {
            string str = "";
            Node<T> current = head;
            while (current != null)
            {
                //Console.Write(current.Data + " -> ");
                str += current.Data;
                str += " ";
                current = current.Next;
            }
            return str;
        }
    }      
}
