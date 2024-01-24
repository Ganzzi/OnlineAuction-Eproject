import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
const CategoryDetailPage = ({ searchParams }: { searchParams: SearchParams }) => {
  const resource = newResource([], 93, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);
  return (
    <>
          <Breadcrumb pageName="Category  Edit" />
          <Pagination meta={resource.meta} />
          <br></br>
          <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">

            <div className="flex flex-col gap-5.5 p-6.5">
              <div>
                <label className="mb-3 block text-black dark:text-white">
                  Name
                </label>
                <input
                  type="text"
                  placeholder="Name"
                  className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                />
              </div>
            </div>
            <div className="flex flex-col gap-5.5 p-6.5">
              <div>
                <label className="mb-3 block text-black dark:text-white">
                Description
                </label>
                <input
                  type="text"
                  placeholder="Description"
                  className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                />
              </div>
              
            </div>
            <div className="flex flex-col gap-5.5 p-6.5">
              <div>
              <a className="inline-flex items-center justify-center rounded-md bg-meta-3 py-4 px-10 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5" href="#">Submit</a>
              </div>
            </div>
          </div>
 
    </>
  );
};

export default CategoryDetailPage;
