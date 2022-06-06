using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public interface IJobQueue {
        public void Push(Action action);
    }
    public class JobQueue : IJobQueue{
        private Queue<Action> _queue = new Queue<Action>();
        private object l_queue = new object();

        public void Push(Action action) {
            lock(l_queue) {
                _queue.Enqueue(action);
            }
        }

        public Action Pop() {
            lock(l_queue) {
                return _queue.Dequeue();
            }
        }

        public List<Action> PopAll() {
            lock(l_queue) {
                List<Action> list = new List<Action>();
                foreach(Action action in _queue) {
                    list.Add(action);
                }

                return list;
            }
        }

        public void Flush() {
            lock(l_queue) {
                foreach(Action action in _queue) {
                    action.Invoke();
                }
            }
        }
    }
}
