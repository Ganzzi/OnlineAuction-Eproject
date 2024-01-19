import Link from "next/link";

interface BreadcrumbProps {
  listPages: Array<Page>;
}

type Page = {
  pageName: string,
  link: string
}

const Breadcrumb = ({ listPages }: BreadcrumbProps) => {
  return (
    <div className="mb-6 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
      {/* <h2 className="text-title-md2 font-semibold text-black dark:text-white">
        {listPages.map(page => page.pageName.toString() + "/")}
      </h2> */}

      <nav>
        <ol className="flex items-center gap-2">
          <li>
            <Link className="font-medium hover:text-primary" href="/">
              Home
            </Link>
          </li>
          <li>
            {listPages.map(page => (
              <Link className="font-medium hover:text-primary" href={page.link}>
                {" / " + page.pageName.toString()}
            </Link>
              ) )} 
          </li>
        </ol>
      </nav>
    </div>
  );
};

export default Breadcrumb;
