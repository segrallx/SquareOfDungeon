// 改变Char的朝向

public class CmdFace: Cmd {
	public Direction mDirction;

	public CmdFace(Direction dir):base() {
		mDirction = dir;
	}

	public override void Do(RoleRender c) {
		c.DirectionSet(mDirction);
	}
}
