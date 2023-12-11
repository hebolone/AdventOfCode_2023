internal record Node<T>(T Item, Node<T>? Ancestor = null);

internal class Tree<T> {

    protected readonly List<Node<T>> _Datas = [];

    public void AddNode(T item, Node<T>? ancestor = null) {
        _Datas.Add(new(item, ancestor));
    }

    public IEnumerable<Node<T>> GetChildren(Node<T> node) => _Datas.Where(n => n.Ancestor == node);

    public Node<T>? GetRoot() => _Datas.FirstOrDefault(i => i.Ancestor == null);

    // public static int GetLevel(this Node<T> node) {
    //     return 0;
    // }

    /*
    
    fun listNode(node: Node<T>) {
        val spaceIndent = " ".repeat(node.getLevel() * 2)
        println("$spaceIndent- ${node.item}")
        _datas.filter { it.ancestor == node }.forEach {
            if (_datas.any { d -> d.ancestor == it })
                listNode(it)
            else
                println("  $spaceIndent- ${it.item}")
        }
    }

    fun getChildren(node: Node<T>): List<Node<T>> = _datas.filter { it.ancestor == node }

    fun getRoot(): Node<T>? = _datas.firstOrNull { it.ancestor == null }

    fun getIterator(): TreeIterator<T> = TreeIterator(this)

    private fun Node<T>.getLevel(): Int {
        var retValue = 0
        var pointer: Node<T>? = this
        while (pointer != null) {
            retValue++
            pointer = pointer.ancestor
        }
        return retValue
    }
    
    */

}