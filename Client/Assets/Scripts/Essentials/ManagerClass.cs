using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ManagerType {
    Updatable,
    Static
}
public enum ManagerIndex {
    Network,

}

public abstract class ManagerClass {
    public ManagerType Type { get; protected set; }

    public virtual void Initialize() { }
    public virtual void Update() { }
}