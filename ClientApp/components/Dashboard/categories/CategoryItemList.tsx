"use client"
import { Category } from '@/types/models/category'
import { CategoryItem } from '@/types/models/categoryItem'
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import React from 'react'

const categoryItems:CategoryItem []=[]

// const categories: Category[]=[
//   {
//     categoryId: 1,
//     categoryName: "2",
//     description: "2",
//     categoryItems : categoryItems,
// },
// {
//   categoryId: 1,
//   categoryName: "2",
//   description: "2",
//   categoryItems : categoryItems,
// },
// {
//   categoryId: 1,
//   categoryName: "2",
//   description: "2",
//   categoryItems : categoryItems,
// },  {
//   categoryId: 1,
//   categoryName: "2",
//   description: "2",
//   categoryItems : categoryItems,
// }
// ]

export type CategoryResponse = {
  category: Category,
  itemCount: number
}
const CategoryItemList = ({data}: {data: CategoryResponse[]}) => {
  const router = useRouter();
  return (
  <div>
                <Link
              href="/dashboard/categories/form"
              className="inline-flex items-center justify-center rounded-md bg-meta-3 py-4 px-5 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-3"
            >
              Create
            </Link>
        <div className="rounded-sm border border-stroke bg-white px-5 pt-6 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:pb-1">
      <div className="max-w-full overflow-x-auto">
        <table className="w-full table-auto">
          <thead>
            <tr className="bg-gray-2 text-left dark:bg-meta-4">
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">
              categoryId
              </th>
              <th className="min-w-[150px] py-4 px-4 font-medium text-black dark:text-white">
              Name
              </th>
              <th className="min-w-[120px] py-4 px-4 font-medium text-black dark:text-white">
              description
              </th>
              <th className="py-4 px-4 font-medium text-black dark:text-white">
              Items
              </th>
              <th className="py-4 px-4 font-medium text-black dark:text-white">
              Action
              </th>
            </tr>
          </thead>
          <tbody>
            {data.map((item, key) => (
              <tr onClick={()=>{
                router.push(`/dashboard/categories/${item.category.categoryId}`)
              }} key={key}>
                <td className="border-b border-[#eee] py-5 px-4 pl-9 dark:border-strokedark xl:pl-11">
                  <h5 className="font-medium text-black dark:text-white">
                    {item.category.categoryId}
                  </h5>
                 
                </td>
                <td className="border-b border-[#eee] py-5 px-4 dark:border-strokedark">
                  <p className="text-black dark:text-white">
                  <p className="text-sm">{item.category.categoryName}</p>
                  </p>
                </td>
                <td className="border-b border-[#eee] py-5 px-4 dark:border-strokedark">
                  <p className="text-black dark:text-white">
                    {item.category.description}
                  </p>
                </td>
                <td className="border-b border-[#eee] py-5 px-4 dark:border-strokedark">
                 <p>
                    {item.itemCount || 0}
                  </p>
                </td>
                <td className="border-b border-[#eee] py-5 px-4 dark:border-strokedark">
                <Link href={`/dashboard/categories/form?categoryId=${item.category.categoryId}`} className="inline-flex items-center justify-center rounded-full bg-black py-4 px-5 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5">
              Edit
            </Link>
             | 
              <Link href="#"className="inline-flex items-center justify-center rounded-full bg-black py-4 px-5 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5">
              Delete
            </Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  </div>
  )
}

export default CategoryItemList