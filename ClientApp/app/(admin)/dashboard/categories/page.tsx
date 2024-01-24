'use client'

import Breadcrumb from "@/components/Dashboard/Breadcrumb";

import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
import CategoryItemList, { CategoryResponse } from "@/components/Dashboard/categories/CategoryItemList";
import { useEffect, useState } from "react";
import axiosService from "@/axiosService";

const CategoriesPage = ({ searchParams }: { searchParams: SearchParams }) => {
  const [data, setData] = useState<CategoryResponse[]>([]);
  
  // 1
  const [resource, setResource] = useState<Resource<CategoryResponse>>(newResource([], 100, searchParams?.page as unknown as number ?? 0, 10))

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await axiosService.get(`/api/Admin/getallCategory?page=${searchParams?.page}&take=10`);

        console.log(res.data);
        
        const _data: CategoryResponse[] = res.data ; 
        setData(_data);



        // 2
        const rsc = newResource(data, 100, searchParams?.page as unknown as number ?? 0, 10);
        
        setResource(rsc);
      } catch (error) {
        console.error('Error fetching categories:', error);
        // Handle the error or provide a fallback mechanism if needed
      }
    };

    fetchData();
  }, []); 
  return (
    <>
      <Breadcrumb pageName="Categories" />
      {/* <Pagination meta={resource.meta} /> */}
      <br /><br />
      <CategoryItemList data={data}/>
       
    </>
  );
};

export default CategoriesPage;
