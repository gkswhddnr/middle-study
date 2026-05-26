using UnityEngine;

public class Solution
{
    public int solution(int totalExp)
    {
        int xp = 100;
        int level = 1;

        while (totalExp >= xp)
        {
            totalExp -= xp;
            xp += 100;
            level += 1;
        }

        return level;
    }
}
