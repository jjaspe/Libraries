using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSystemLibrary.Classes
{
    /* Check-Effects: Each action will have a series of checks that trigger some effects.
     * Each of these checks might be dependent or independent of others. A group of dependent checks is grouped
     * in a checkList, and all this groups (checkLists), independent of each other, are grouped in itsChecks (a checkList[]).
     * Each check has a threshhold member(a int[]), that represents the outcomes of the check (0=failure,1=small successs,2=success, etc), so
     * each checkList will have a list of threshholds(an int[][]) that represent the outcomes of all its checks.
     * A series of effects that will take place simultaneously are grouped in an effectList. The effectList will have a threshhold member (an int[])
     * that represents the outcomes of the checks that make the effects in this effectList take place.
     * Since each group of dependent checks (one checkList) can have a group of effects, and there are several of those checkLists,
     * and for each checkList there are several combinations of outcomes, actions must be a effectList[][].
     * 
     * So effectList[i][] is the all the effectLists that can take place depending on the outcome of checkList[i]
     * 
     * */
    public class WeaponAction : Action
    {
        public string itsName;
        private checkList[] itsChecks;        
        private effectList[][] itsEffectLists;

        #region CONSTRUCTORS
        public WeaponAction()
        {
            itsName = " ";
        }
        #endregion

        #region ACCESSORS
        public checkList[] ItsChecks
        {
            get { return itsChecks; }
            set { itsChecks = value; }
        }
        public effectList[][] ItsEffectLists
        {
            get { return itsEffectLists; }
            set { itsEffectLists = value; }
        }
        #endregion 

        public bool isInitialized()
        {
            return (itsChecks != null && itsEffectLists != null);
        }
        public WeaponAction copy(WeaponAction wep)
        {
            WeaponAction copyWeaponAction =(WeaponAction)base.copy(wep);
            if (isInitialized())
            {
                copyWeaponAction.itsName = this.itsName;

                copyWeaponAction.itsChecks = new checkList[this.itsChecks.Length];
                for (int i = 0; i < this.itsChecks.Length; i++)
                    copyWeaponAction.itsChecks[i] = this.itsChecks[i].copy();

                for (int i = 0; i < itsEffectLists.Length; i++)
                {
                    for (int j = 0; j < itsEffectLists[i].Length; j++)
                        copyWeaponAction.itsEffectLists[i][j] = this.itsEffectLists[i][j].copy();
                }
            }           
            return copyWeaponAction;
        }

       
        /* For every list of checks, get the threshholds. Then
         * for the array of lists of effects with the same index
         as the list of checks, get the one list with the same threshholds*/
        public virtual bool doAction(bool doChecks=true,bool doEffects=true)
        {
            int[] currentThreshholds;
            effectList foundList=new effectList();
            for (int i = 0; i < itsChecks.Length; i++)
            {
                //Get threshholds for this list, if doChecks=false, use last threshHolds
                currentThreshholds = doChecks?
                    itsChecks[i].doChecks():itsChecks[i].LastThreshholds;
                if (doEffects)
                {
                    foundList=WeaponAction.getEffectListByThreshhold
                        (itsEffectLists[i], currentThreshholds);
                    if (foundList != null)
                        foundList.doEffects();
                }
            }                    
            return true;
        }
        
        
        public bool isEffect(string name)
        {           
            return false;
        }

        // STATIC METHODS
        public static effectList getEffectListByThreshhold(effectList[] lists, int[] th)
        {
            if (lists == null || th == null)
                return null;
            for (int j = 0; j < lists.Length; j++)
            {
                //Check which list of effects has the same threshholds
                if (lists[j].checkThreshholds(th))
                {
                    return lists[j];
                }
            }
            return null;
        }
        
    }
}
