using System.Linq;
using System.Net.Http.Formatting;
using Izio.Umbraco.Emailer.Repositories;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Izio.Umbraco.Emailer.Controllers
{
    [Tree("izioEmailer", "emailer", "Emailer")]
    [PluginController("Emailer")]
    public class EmailerTreeController : TreeController
    {
        private readonly FormRepsoitory _repository;

        public EmailerTreeController()
        {
            _repository = new FormRepsoitory();
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            nodes.AddRange(_repository.GetAll().Select(contactForm => CreateTreeNode(contactForm.Id.ToString(), "-1", queryStrings, contactForm.Name)));

            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            if (id == "-1")
            {
                menu.Items.Add<ActionNew>("Create");
                menu.Items.Add<ActionRefresh>("Refresh");

                return menu;
            }

            menu.Items.Add<ActionDelete>("Delete");

            return menu;
        }
    }
}
