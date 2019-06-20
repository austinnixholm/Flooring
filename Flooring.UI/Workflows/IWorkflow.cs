using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.UI.IO;

namespace Flooring.UI.Workflows
{
    /// <summary>
    /// Base implementation for a workflow method to be used in correlation with the Flooring Manager UI
    /// </summary>
    public interface IWorkflow
    {
        void Execute(FlooringOrders ui, Output output, Input input);

    }
}
