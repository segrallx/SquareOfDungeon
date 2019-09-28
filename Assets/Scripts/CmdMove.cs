// 改变Char的朝向

public class CmdMove: Cmd {
	public Direction mDirction;

	public CmdMove(Direction dir):base() {
		mDirction = dir;
	}

	public override void Do(RoleRender c) {
		c.MoveSet(mDirction);
	}
}
