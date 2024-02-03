'use client'

import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
import UserList from "@/components/Dashboard/users/UserList";
import { useEffect, useState } from "react";
import axiosService from "@/services/axiosService";
import { User } from "@/types/models/user";

export type UserData = {
  user: User,
  ratings: number,
  avgRate: number,
  bidCount: number
}
const UsersPage = ({ searchParams }: { searchParams: SearchParams }) => {
  const [resource, setResource] = useState<Resource<UserData>>(newResource([], 10, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10))
  
  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await axiosService.get(`/api/Admin/GetListUserWithRatingAndBidCount${searchParams?.page != undefined ? "?page=" + searchParams?.page : ""}`);
        
        const _data: UserData[] = res.data.userData; 
        const count = res.data.count;

        const src = newResource(_data, count, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);

        setResource(src);
      } catch (error) {
        console.error('Error fetching categories:', error);
        // Handle the error or provide a fallback mechanism if needed
      }
    };

    fetchData();
  }, [searchParams]); 

  return (
    <>
      <Breadcrumb pageName="Users" />
      <Pagination meta={resource.meta} />
      <br /><br />
      <UserList userData={resource.data} />
    </>
  );
};

export default UsersPage;
