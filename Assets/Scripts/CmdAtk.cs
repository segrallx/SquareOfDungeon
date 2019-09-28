// 改变Char的朝向
public class CmdAtk: Cmd {

	public enum Idx {
		Primary = 1,
		Secondary,
	};


	// 1-主武器
	// 2-副武器
	public Idx mAtkIdx=0;

	public CmdAtk(Idx mIdx):base() {
		mAtkIdx = mIdx;
	}

	public override void Do(RoleRender c) {

		switch(mAtkIdx) {
			case Idx.Primary:
				doPrimary(c);
				break;
			case Idx.Secondary:
				doSecondary(c);
				break;
		}

	}

	private void doPrimary(RoleRender c) {
		c.AtkSet();
	}

	private void doSecondary(RoleRender c) {
		c.DefenseSet();
	}
}
