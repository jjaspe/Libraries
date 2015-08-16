using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSystemLibrary.Classes
{
    public class Weapon : TBCComponents
    {
        public string itsName;
        characterList itsUsers;
        characterList itsTargets;
        List<WeaponAction> itsActions;
        WeaponAction[] itsChoosableActions;
        /*Double dimensional list of int, which holds the percent chance that an action
		will be called after another one (i.e. subs_list[i][j] gives the chance action j will happen after i).*/
        int[,] itsSubActions;
        bool[] target_Users;

        // Skills this weapon modifies, and amount by which it modifies them.
        propertyList itsModifiedSkills;
        Coefficient[] itsSkillModifiers;

        /*******************CONSTRUCTORS*********************/
        public Weapon()
        {
            itsName = " ";
            itsUsers = new characterList();
            itsTargets = new characterList();
            itsActions = new List<WeaponAction>();
            itsChoosableActions = new WeaponAction[Globals.ACTION_LIMIT];
            itsSubActions = new int[Globals.ACTION_LIMIT, Globals.ACTION_LIMIT];
            itsModifiedSkills = new propertyList();
            itsSkillModifiers = new Coefficient[Globals.SKILL_LIMIT];

            initializeLists();
            
        }
        private void initializeLists()
        {
            /* Set all chances to 0, so that initially no action will automatically
             * be called after any other */
            for (int i = 0; i < Globals.ACTION_LIMIT; i++)
            {
                for (int j = 0; j < Globals.ACTION_LIMIT; j++)
                    itsSubActions[i, j] = 0;
            }
            target_Users = new bool[Globals.ACTION_LIMIT];
            for (int j = 0; j < Globals.ACTION_LIMIT; j++)
            {
                target_Users[j] = false;
            }
            for (int j = 0; j < Globals.ACTION_LIMIT; j++)
            {
                itsChoosableActions[j] = new WeaponAction();
            }
        }
        public Weapon copy(Weapon copyWeapon)
        {
            copyWeapon = (Weapon)base.copy(copyWeapon);
            copyWeapon.itsName = this.itsName;

            for (int i = 0; i < itsActions.Count; i++)
                copyWeapon.itsActions[i] = this.itsActions[i].copy(new WeaponAction());

            for (int i = 0; i < itsChoosableActions.Length; i++)
                copyWeapon.itsChoosableActions[i] = this.itsChoosableActions[i].copy(new WeaponAction());

            copyWeapon.itsUsers = this.itsUsers.copy();

            copyWeapon.itsTargets = this.itsTargets.copy();

            for (int i = 0; i < target_Users.Length; i++)
                copyWeapon.target_Users[i] = this.target_Users[i];

                copyWeapon.itsModifiedSkills = this.itsModifiedSkills.copy();

             for (int i = 0; i < itsSkillModifiers.Length; i++)
                copyWeapon.itsSkillModifiers[i] = this.itsSkillModifiers[i];

             return copyWeapon;
        }

        #region ACCESSORS
        public Character[] getAllUsers()
        {
            return (Character[])itsUsers.toArray();
        }
        public bool addUser(Character newUser)
        {
            if (itsUsers.Count < Globals.CHARACTER_LIMIT)
            {
                itsUsers.add(newUser);
                return true;
            }
            return false;
        }
        public bool removeUser(Character oldUser)
        {
            if (isUser(oldUser.Name))
            {
                itsUsers.Remove(oldUser);
                return true;
            }
            else
                return false;
        }
        public bool isUser(string name)
        {
            return itsUsers.find(name) != null;
        }
        public Character getUser(string name)
        {
            return itsUsers.find(name);
        }

        public Character[] getAllTargets()
        {
            return (Character[])itsTargets.toArray();
        }
        public bool addTarget(Character newTarget)
        {
            if (itsTargets.Count < Globals.CHARACTER_LIMIT)
            {
                itsTargets.add(newTarget);
                return true;
            }
            return false;
        }
        public bool removeTarget(Character oldTarget)
        {
            if (isTarget(oldTarget.Name))
            {
                itsTargets.Remove(oldTarget);
                return true;
            }
            else
                return false;
        }
        public bool isTarget(string name)
        {
            return itsTargets.find(name) != null;
        }
        public Character getTarget(string name)
        {
            return itsTargets.find(name);
        }

        public int actionIndex(string name)
        {
            for (int i = 0; i < Globals.ACTION_LIMIT; i++)
            {
                if (itsActions[i].itsName == name)
                {
                    return i;
                }
            }
            return Globals.ACTION_LIMIT;
        }
        public bool addAction(WeaponAction newAction)
        {
            itsActions.Add(newAction);
            return false;
        }
        public bool removeAction(WeaponAction oldAction)
        {
            int i = actionIndex(oldAction.itsName);
            if (i < Globals.ACTION_LIMIT)
            {
                itsActions[i].is_empty = true;
                return true;
            }
            else
                return false;
        }
        public bool isAction(string name)
        {
            if (actionIndex(name) < Globals.ACTION_LIMIT)
                return true;
            else
                return false;
        }
        /* If Weapon_Action wanted is not in actions array, a
         * new empty Weapon_Action will be returned. */        
        public WeaponAction getAction(string name)
        {
            foreach (WeaponAction Action in itsActions)
            {
                if (Action.itsName == name)
                    return Action;
            }
            return new WeaponAction();
        }

        public int choosableActionIndex(string name)
        {
            for (int i = 0; i < Globals.ACTION_LIMIT; i++)
            {
                if (itsChoosableActions[i].itsName == name)
                {
                    return i;
                }
            }
            return Globals.ACTION_LIMIT;
        }
        public bool addChoosableAction(WeaponAction newAction)
        {
            for (int i = 0; i < Globals.ACTION_LIMIT; i++)
            {
                if (itsChoosableActions[i].is_empty)
                {
                    itsChoosableActions[i] = newAction;
                    itsChoosableActions[i].is_empty = false;
                    return true;
                }
            }
            return false;
        }
        public bool removeChoosableAction(WeaponAction oldAction)
        {
            int i = choosableActionIndex(oldAction.itsName);
            if (i < Globals.ACTION_LIMIT)
            {
                itsChoosableActions[i].is_empty = true;
                return true;
            }
            else
                return false;
        }
        public bool isChoosableAction(string name)
        {
            if (choosableActionIndex(name) < Globals.ACTION_LIMIT)
                return true;
            else
                return false;
        }
        /* If Weapon_Action wanted is not in choosable actions array, a
         * new empty Weapon_Action will be returned. */        
        public WeaponAction getChoosableAction(string name)
        {
            foreach (WeaponAction Action in itsChoosableActions)
            {
                if (Action.itsName == name)
                    return Action;
            }
            return new WeaponAction();
        }

        public Skill[] getAllModifiedSkills()
        {
            return (Skill[])itsModifiedSkills.toArray();
        }
        public bool addModifiedSkill(Skill newModifiedSkill)
        {
            if (itsModifiedSkills.Count < Globals.ATTRIBUTE_LIMIT)
            {
                itsModifiedSkills.add(newModifiedSkill);
                return true;
            }
            return false;
        }
        public bool removeModifiedSkill(Skill oldModifiedSkill)
        {
            if (isModifiedSkill(oldModifiedSkill.Name))
            {
                itsModifiedSkills.Remove(oldModifiedSkill);
                return true;
            }
            else
                return false;
        }
        public bool isModifiedSkill(string name)
        {
            return itsModifiedSkills.find(name) != null;
        }
        public Skill getModifiedSkill(string name)
        {
            return (Skill)itsModifiedSkills.find(name);
        }
       
        //Return 0 if skill not found
        public int getSkillModifier(string name)
        {
            foreach (Coefficient coef in itsSkillModifiers)
            {
                if (coef.VariableName == name)
                    return coef.getValue();
            }
            return 0;
        }
        public bool targetUser(int i)
        {
            if (i < Globals.ACTION_LIMIT)
            {
                target_Users[i] = true;
                return true;
            }
            else
                return false;
        }
        public bool targetTarget(int i)
        {
            if (i < Globals.ACTION_LIMIT)
            {
                target_Users[i] = false;
                return true;
            }
            else
                return false;
        }


        #endregion

        public bool setSubActionChance(WeaponAction First, WeaponAction Second, int Chance)
        {
            int i = actionIndex(First.itsName);
            int j = actionIndex(Second.itsName);
            if (i < Globals.ACTION_LIMIT && j < Globals.ACTION_LIMIT && Chance>=0 && Chance <=100)
            {
                itsSubActions[i, j] = Chance;
                return true;
            }
            else
                return false;
        }
        public bool doAction(string actionName)
        {
            int i = actionIndex(actionName);
            if (itsActions[i] != null)
                itsActions[i].doAction();
            /*
            Character Target = new Character();
            Character[] tempUsers = itsUsers.toArray();
            Character[] tempTargets = itsTargets.toArray();
            if (i >= Globals.ACTION_LIMIT)
                return false;
            Random random=new Random();
            int toChance;
            for (int j = 0; j < Globals.CHARACTER_LIMIT; j++)
            {
                if (target_Users[i])
                    Target = tempUsers[j];
                else
                    Target = tempTargets[j];
                if (!tempTargets[j].is_empty)//only act on initialized characters
                {
                    itsActions[i].doAction(Target);*/
                    /* Walks through all subactions, rolling against their chance
                     * to be performed after action i. */
            /*
                    for (int k = 0; k < Globals.ACTION_LIMIT; k++)
                    {
                        toChance = random.Next();
                        if (toChance <= itsSubActions[i, k])
                            itsActions[k].doAction(Target);
                    }
                }
            }*/
            return true;
        }
    }
}
