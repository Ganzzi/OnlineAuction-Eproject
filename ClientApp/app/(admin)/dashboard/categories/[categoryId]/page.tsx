'use client'

import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import { Resource, newResource } from "@/types/resource";
import { SearchParams } from "@/types/next";
import Pagination from "@/components/common/Pagination/Pagination";
import ItemList from "@/components/Dashboard/categories/ItemList";
import { Item } from "@/types/models/item";
import { Category } from "@/types/models/category";
import { useEffect, useState } from "react";
import axiosService from "@/services/axiosService";
import CategoryForm from "@/components/Dashboard/categories/CategoryForm";
import { usePathname, useRouter } from "next/navigation";
import CategoryFilter from "@/components/Dashboard/categories/CategoryFilter";

export type CategoryData = {
  item: Item,
  belong: boolean
}

interface CategoryDetailPageProps {
  searchParams: SearchParams;
  params: { categoryId: number };
}

const CategoryDetailPage: React.FC<CategoryDetailPageProps> = ({ searchParams, params: { categoryId } }) => {
  const [category, setCategory] = useState<Category | null>(null);
  const [resource, setResource] = useState<Resource<CategoryData>>(
    newResource([], 10, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10)
  )
  const router = useRouter();
  const pathname = usePathname();
  const [search, setSearch] = useState<string>(searchParams?.search as string || '');
  const [belongToCategory, setBelongToCategory] = useState<string | null>(searchParams?.belongToCategory as string || null);


  useEffect(() => {
    const fetchData = async () => {
      try {
        const newSearchParams = new URLSearchParams(searchParams);
        newSearchParams.set('search', search);
        if (belongToCategory !== null) {
          newSearchParams.set('belongToCategory', belongToCategory);
        } else {
          newSearchParams.delete('belongToCategory');
        }

        router.push(`${pathname}?${newSearchParams}`);

        const res = await axiosService.get(`/api/Admin/CategoryDetailWithListCategoryItems/${categoryId}`, {
          params: {
            search,
            belongToCategory,
            page: resource.meta.current_page,
          },
        });

        console.log(res.data);
        

        const cate: Category = res.data.category;
        const items: CategoryData[] = res.data.items;
        const count = res.data.count;

        setCategory(cate);
        setResource(newResource(items, count, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10));
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, [categoryId, search, belongToCategory]);

  const handleSearchChange = (value: string) => {
    setSearch(value);
  };

  const handleSelectChange = (value: string) => {
    setBelongToCategory(value);
  };

  return (
    <>
      <Breadcrumb pageName="Category Edit" />
      <br />

      {/* CategoryForm component */}
      {category && <CategoryForm category={category} />}

      <hr />
      <br />

      {category && (
        <>
          <div className="flex flex-row items-center justify-center space-x-5">
            {/* CategoryFilter component */}
            <CategoryFilter
              search={search}
              belongToCategory={belongToCategory || ''}
              onSearchChange={handleSearchChange}
              onSelectChange={handleSelectChange}
            />

            {/* Pagination component */}
            <Pagination meta={resource.meta} />
          </div>

          {/* ItemList component */}
          <ItemList items={resource.data} categoryId={categoryId} />
        </>
      )}
    </>
  );
};



export default CategoryDetailPage;
