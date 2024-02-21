'use client'

import ReactPaginate from 'react-paginate'
import React, { useEffect, useState } from 'react'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'
import { convertToURLSearchParams } from '@/utils'

type Props = {
  currentPage: number;
  lastPage: number;
  setPage?: (page: number) => void;
}

export default function Paginate(props: Props) {
  const { currentPage, lastPage, setPage } = props
  const [pageIndex, setPageIndex] = useState(currentPage - 1)
  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()

  useEffect(() => {
    setPageIndex(currentPage - 1)
  }, [currentPage])

  return (
    <div className="">
      <ReactPaginate
        forcePage={pageIndex}
        pageCount={lastPage}
        marginPagesDisplayed={1}
        pageRangeDisplayed={3}
        containerClassName="mb-0 flex flex-row "
        pageClassName="px-1 mx-1 py-1 rounded-2xl"
        breakClassName=""
        previousClassName=""
        nextClassName=""
        previousLinkClassName="hover:bg-primary px-2 py-1 rounded-xl"
        pageLinkClassName="hover:bg-success px-2 mx-1 py-1 rounded-lg"
        breakLinkClassName=""
        nextLinkClassName="hover:bg-primary px-2 py-1 rounded-xl"
        previousLabel="‹"
        nextLabel="›"
        activeClassName="active text-red-500 bg-success"
        disabledClassName="disabled"
        onPageChange={(selectedItem) => {
          const page = selectedItem.selected + 1

          if (setPage) {
            setPage(page)
          }

        const newSearchParams = convertToURLSearchParams(searchParams);

          newSearchParams.set('page', page.toString())

          router.push(`${pathname}?${newSearchParams}`)
        }}
      />
    </div>
  )
}
