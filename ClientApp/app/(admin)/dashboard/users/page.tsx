import Breadcrumb from "@/components/Dashboard/Breadcrumb";

import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
import UserList from "@/components/Dashboard/users/UserList";

const UsersPage = ({ searchParams }: { searchParams: SearchParams }) => {
  const resource = newResource([], 93, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);
  return (
    <>
      <Breadcrumb pageName="Users" />
      <Pagination meta={resource.meta} />
      <br /><br />
      <UserList />
    </>
  );
};

export default UsersPage;
