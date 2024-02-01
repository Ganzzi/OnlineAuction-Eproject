'use client'

import Breadcrumb from "@/components/Dashboard/Breadcrumb";

import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
import CategoryList, { CategoryResponse } from "@/components/Dashboard/categories/CategoryList";
import { useEffect, useState } from "react";
import axiosService from "@/services/axiosService";

const CategoriesPage = ({ searchParams }: { searchParams: SearchParams }) => {
  const [data, setData] = useState<CategoryResponse[]>([]);
  
  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await axiosService.get(`/api/Admin/getallCategory?page=${searchParams?.page}&take=10`);

        console.log(res.data);
        
        const _data: CategoryResponse[] = res.data.categorylist; 
        setData(_data);
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
      <CategoryList data={data}/>
       
    </>
  );
};

export default CategoriesPage;
