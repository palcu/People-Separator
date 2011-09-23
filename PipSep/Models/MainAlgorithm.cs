using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipSep.Models
{
    public class ReturnedThing
    {
        public List<List<ApplicationCV>> Groups = new List<List<ApplicationCV>>();
        public List<ApplicationCV> Outside = new List<ApplicationCV>();
    }
    public class MainAlgorithm
    {
        public static ReturnedThing GetResults(List<PairAccountsValue> values, List<int> groups, List<ApplicationCV> acceptedPeople)
        {
            List<List<ApplicationCV>> results = new List<List<ApplicationCV>>();

            int nGroup = 0;

            while (nGroup < groups.Count && groups[nGroup] <= acceptedPeople.Count)
            {
                List<ApplicationCV> local = new List<ApplicationCV>();

                //if we have only one person... select the first one
                if (groups[nGroup] == 1)
                {
                    local.Add(acceptedPeople[0]);
                    acceptedPeople.RemoveAt(0);
                    results.Add(local);
                    break;
                }

                //select starting group (it was not selected)

                foreach (var i in values)
                {
                    if (acceptedPeople.Contains(i.First) && acceptedPeople.Contains(i.Second))
                    {
                        local.Add(i.First);
                        local.Add(i.Second);
                        acceptedPeople.Remove(i.First);
                        acceptedPeople.Remove(i.Second);

                        //acceptedPeople.RemoveAll(r => r.UserName == values[0].First.UserName);
                        //acceptedPeople.RemoveAll(r => r.UserName == values[0].Second.UserName);
                        break;
                    }
                }

                //search for other people to enter the group
                while (groups[nGroup] > local.Count)
                {
                    int max = -1;
                    ApplicationCV best = new ApplicationCV();

                    foreach (var candidate in acceptedPeople)
                    {
                        //test candidate if it has the best chances
                        int localsum = 0;
                        foreach (var member in local)
                        {
                            //search local thing
                            List<PairAccountsValue> pair = values.Where(r=> (r.First.UserName==member.UserName && r.Second.UserName==candidate.UserName) || (r.Second.UserName==member.UserName && r.First.UserName==candidate.UserName)).ToList();
                            if (pair.Count == 1)
                            {
                                localsum += pair[0].Value;
                            }
                        }
                        if (localsum > max)
                        {
                            max = localsum;
                            best = candidate;
                        }
                    }

                    local.Add(best);
                    acceptedPeople.Remove(best);
                }
                results.Add(local);
                ++nGroup;
            }

            ReturnedThing returned = new ReturnedThing();
            returned.Groups = results;
            returned.Outside = acceptedPeople;
            return returned;
        }
    }
}