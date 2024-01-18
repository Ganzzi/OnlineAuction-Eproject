import Breadcrumb from "@/components/Dashboard/Breadcrumb";

import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
import CategoryItemList from "@/components/Dashboard/categories/CategoryItemList";

const CategoriesPage = ({ searchParams }: { searchParams: SearchParams }) => {
  const resource = newResource([], 93, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);
  return (
    <>
      <Breadcrumb pageName="Categories" />
      <Pagination meta={resource.meta} />
      <br /><br />
      <CategoryItemList/>
    </>
  );
};

export default CategoriesPage;
